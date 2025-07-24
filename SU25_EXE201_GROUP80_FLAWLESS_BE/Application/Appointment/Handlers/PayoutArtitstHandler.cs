using Application.Appointment.Commands;
using Application.Appointment.Responses;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Handlers
{
    public class PayoutArtitstHandler : IRequestHandler<PayoutArtitstCommand, PayoutArtitstResponse>
    {
        IUnitOfWork _unitOfWork;
        public PayoutArtitstHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PayoutArtitstResponse> Handle(PayoutArtitstCommand request, CancellationToken cancellationToken)
        {
            var response = new PayoutArtitstResponse();

            var transaction = (await _unitOfWork.TransactionRepository.FindAsync(
                                t => t.Id == request.TransactionId,
                                q => q.Include(t => t.Appointment))).FirstOrDefault();
            if (transaction == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy giao dịch.";
                return response;
            }

            if (transaction.TransactionType != TransactionType.CancellationPayoutArtist 
                )
            {
                response.IsSuccess = false;
                response.Message = "Giao dịch không thuộc loại chi trả cho Artist.";
                return response;
            }
            if (transaction.TransactionStatus != TransactionStatus.Pending)
            {
                response.IsSuccess = false;
                response.Message = "Giao dịch đã được xác nhận hoặc trạng thái không hợp lệ.";
                return response;
            }

            // Mark transaction as completed
            transaction.TransactionStatus = Domain.Enum.TransactionStatus.Completed;
            transaction.PaymentProviderTxnId = request.TransactionCode;

            _unitOfWork.TransactionRepository.Update(transaction);

            var artistProgress = (await _unitOfWork.ArtistProgressRepository
                .FindAsync(ap => ap.Artist.Id == transaction.Appointment.ArtistMakeupId)).FirstOrDefault();

            artistProgress.TotalReceive += transaction.Amount; 
            _unitOfWork.ArtistProgressRepository.Update(artistProgress);

     
            var commission = new Commission
            {
                AppointmentId = transaction.AppointmentId,
                CommissionType = CommissionType.CommissionCancelledAppoinment,
                Rate = 0.10m,
                Amount = transaction.Amount
            };
            _unitOfWork.CommissionRepository.AddAsync(commission);

            var appoinment = await _unitOfWork.AppointmentRepository.GetByIdAsync((Guid)transaction.AppointmentId);
            appoinment.Status = AppointmentStatus.Refunded;
            _unitOfWork.AppointmentRepository.Update(appoinment);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Xác nhận chi trả cho nghệ sĩ thành công.";

            return response;
        }
    }
}
