using Application.Appointment.Commands;
using Application.Appointment.Responses;
using Castle.Core.Configuration;
using Domain.Entities;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Payment;
using MediatR;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Appointment.Handlers
{
    public class BookingAppoinmentHandler : IRequestHandler<BookingAppoinmentCommand, BookingAppoinmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayOsService _payOsService;
        public BookingAppoinmentHandler(
            IUnitOfWork unitOfWork,
            IPayOsService payOsService)
        {
            _unitOfWork = unitOfWork;
            _payOsService = payOsService;
        }
        public async Task<BookingAppoinmentResponse> Handle(BookingAppoinmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BookingAppoinmentResponse();

            var artist = (await _unitOfWork.UserAppRepository.FindAsync(u => u.Id == request.ArtistMakeupId)).FirstOrDefault();
            var customer = (await _unitOfWork.UserAppRepository.FindAsync(u => u.Id == request.CustomerId)).FirstOrDefault();

            if (artist == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Artist makeup không tồn tại.";
                return response;
            }
            if (customer == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Khách hàng không tồn tại.";
                return response;
            }

            decimal totalAmountBeforeDiscount = 0;
            var appointmentDetails = new List<Domain.Entities.AppointmentDetail>();
            string serviceOptionName = null;

            foreach (var detailRequest in request.AppointmentDetails)
            {
                var serviceOption = (await _unitOfWork.ServiceOptionRepository.FindAsync(s => s.Id == detailRequest.ServiceOptionId)).FirstOrDefault();
                if (serviceOption == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"ServiceOption với Id {detailRequest.ServiceOptionId} không tồn tại.";
                    return response;
                }

                serviceOptionName = serviceOption.Name;

                decimal unitPrice = serviceOption.Price;
                if (serviceOption.DiscountPercent.HasValue && serviceOption.DiscountPercent.Value > 0)
                {
                    unitPrice = unitPrice * (100 - serviceOption.DiscountPercent.Value) / 100;
                }

                appointmentDetails.Add(new Domain.Entities.AppointmentDetail
                {
                    ServiceOptionId = detailRequest.ServiceOptionId,
                    Quantity = detailRequest.Quantity,
                    Note = detailRequest.Note,
                    UnitPrice = unitPrice
                });

                totalAmountBeforeDiscount += unitPrice * detailRequest.Quantity;
            }

            decimal depositForApp = totalAmountBeforeDiscount * Domain.Constrans.CommissionConstrans.CompletetionPercentage;

            Domain.Entities.Voucher? appliedVoucher = null;
            decimal totalDiscount = 0;
            decimal totalAmountAfterDiscount = totalAmountBeforeDiscount;

            if (request.VoucherId.HasValue)
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(request.VoucherId.Value);
                if (voucher == null || !voucher.IsActive || voucher.StartDate > DateTime.UtcNow || voucher.EndDate < DateTime.UtcNow)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Voucher không hợp lệ hoặc đã hết hạn.";
                    return response;
                }
                if (voucher.CreatordId != request.ArtistMakeupId)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Voucher không thuộc về nghệ sĩ makeup này.";
                    return response;
                }
                if (voucher.ServiceOption.HasValue)
                {
                    bool applies = appointmentDetails.Any(d => d.ServiceOptionId == voucher.ServiceOption.Value);
                    if (!applies)
                    {
                        response.IsSuccess = false;
                        response.ErrorMessage = "Voucher không áp dụng cho dịch vụ đã chọn.";
                        return response;
                    }
                }
                bool meetsConditions = voucher.DiscountStype switch
                {
                    DiscountStyle.Quantity => voucher.MinQuantity == null || appointmentDetails.Sum(x => x.Quantity) >= voucher.MinQuantity,
                    DiscountStyle.TotalAmount => voucher.MinTotalAmount == null || totalAmountBeforeDiscount >= voucher.MinTotalAmount,
                    _ => true
                };
                if (!meetsConditions)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Không đủ điều kiện để sử dụng voucher.";
                    return response;
                }


                if (voucher.DiscountStype == DiscountStyle.Quantity)
                {
                    int totalQuantity = appointmentDetails.Sum(x => x.Quantity);
                    if (voucher.MinQuantity.HasValue && totalQuantity < voucher.MinQuantity.Value)
                    {
                        response.IsSuccess = false;
                        response.ErrorMessage = $"Voucher yêu cầu tối thiểu {voucher.MinQuantity.Value} dịch vụ.";
                        return response;
                    }
                }
                else if (voucher.DiscountStype == DiscountStyle.TotalAmount)
                {
                    if (voucher.MinTotalAmount.HasValue && totalAmountBeforeDiscount < voucher.MinTotalAmount.Value)
                    {
                        response.IsSuccess = false;
                        response.ErrorMessage = $"Voucher yêu cầu tổng tiền tối thiểu {voucher.MinTotalAmount.Value:#,##0} VNĐ.";
                        return response;
                    }
                }

                totalDiscount = voucher.DiscountType switch
                {
                    DiscountType.Percentage => totalAmountBeforeDiscount * voucher.DiscountValue / 100,
                    DiscountType.Amount => voucher.DiscountValue,
                    _ => 0
                };

                totalDiscount = Math.Min(totalDiscount, totalAmountBeforeDiscount);
                totalAmountAfterDiscount -= totalDiscount;


                _unitOfWork.VoucherRepository.Update(voucher);
                appliedVoucher = voucher;
            }

            
            decimal amountToPayForArtist = totalAmountAfterDiscount - depositForApp;


            var items = appointmentDetails.Select(d =>
                new ItemData(
                    name: $"Service {serviceOptionName}",
                    quantity: d.Quantity,
                    price: (int)d.UnitPrice
                )
            ).ToList();

            long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)depositForApp,
                description: $"{customer.TagName}",
                items: items,
                cancelUrl: "https://web.facebook.com/phan.diem.212245",
                returnUrl: "https://web.facebook.com/tiennvbmthttps://web.facebook.com/tiennvbmt"
            );

            CreatePaymentResult createPayment;
            try
            {
                createPayment = await _payOsService.CreatePaymentLinkAsync(paymentData);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Lỗi khi tạo link thanh toán: {ex.Message}";
                return response;
            }

            if (createPayment == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Không thể tạo link thanh toán.";
                return response;
            }

            var appointment = new Domain.Entities.Appointment
            {
                ArtistMakeupId = request.ArtistMakeupId,
                CustomerId = request.CustomerId,
                AppointmentDate = request.AppointmentDate,
                Address = request.Address,
                Note = request.Note,
                Status = AppointmentStatus.Pending,
                TotalAmount = totalAmountAfterDiscount + totalDiscount,
                TotalDiscount = totalDiscount,
                TotalAmountAfterDiscount = totalAmountAfterDiscount,
                DepositForApp = depositForApp,
                AmountToPayForArtist = amountToPayForArtist,
                VoucherId = appliedVoucher?.Id,
                AppointmentsDetails = appointmentDetails
            };
            _unitOfWork.AppointmentRepository.AddAsync(appointment);

            foreach (var detail in appointmentDetails)
            {
                detail.AppointmentId = appointment.Id;
                _unitOfWork.AppointmentDetailRepository.AddAsync(detail);
            }

            var transaction = new Domain.Entities.Transaction
            {
                AppointmentId = appointment.Id,
                UserId = request.CustomerId,
                ArtistId = request.ArtistMakeupId,
                Amount = depositForApp,
                TransactionType = TransactionType.CustomerPayment,
                TransactionStatus = Domain.Enum.TransactionStatus.Pending,
                PaymentProvider = "PayOS",
                PaymentProviderTxnId = orderCode.ToString(),
                Note = $"Giao dịch thanh toán lịch hẹn #{appointment.Id} qua PayOS"
            };
            _unitOfWork.TransactionRepository.AddAsync(transaction);

            var artistAvailability = new Domain.Entities.ArtistAvailability
            {
                Artist = artist,
                InvailableDateStart = request.StartTime,
                InvailableDateEnd = request.EndTime,
                Note = appointment.Id.ToString(),
                Status = AvailabilityStatus.Booked,
            };
            _unitOfWork.ArtistAvailabilityRepository.AddAsync(artistAvailability);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.PaymentUrl = createPayment.checkoutUrl;
            response.AppointmentId = appointment.Id;

            return response;
        }
    }
}
