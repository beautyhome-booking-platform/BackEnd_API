using Application.Appointment.Commands;
using Application.Appointment.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Handlers
{
    public class CancelledBookingHandler : IRequestHandler<CancelledBookingCommand, CancelledBookingResponse>
    {
        IUnitOfWork _unitOfWork;
        public CancelledBookingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CancelledBookingResponse> Handle(CancelledBookingCommand request, CancellationToken cancellationToken)
        {
            var response = new CancelledBookingResponse();

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Không tìm thấy lịch hẹn.";
                return response;
            }

            // 2. Lấy thông tin ArtistAvailability
            var artistAvailability = (await _unitOfWork.ArtistAvailabilityRepository
                .FindAsync(a => a.Note == request.AppointmentId.ToString()))
                .FirstOrDefault();

            if (artistAvailability == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Không tìm thấy thời gian làm việc tương ứng với lịch hẹn.";
                return response;
            }

            // 3. Kiểm tra thời gian
            var now = DateTime.UtcNow;
            var timeDiff = appointment.AppointmentDate - now;

            Domain.Enum.TransactionType transactionType;
            decimal transactionAmount;
            string note;

            if (timeDiff.TotalDays >= 1)
            {
                appointment.Status = Domain.Enum.AppointmentStatus.WaitRefund;
                transactionType = Domain.Enum.TransactionType.Refund;
                note = $"Giao dịch hoàn tiền cho Customer do hủy lịch hẹn #{appointment.Id} trước 1 ngày.";
                transactionAmount = appointment.DepositForApp;
            }
            else
            {
                appointment.Status = Domain.Enum.AppointmentStatus.Canceled;
                transactionType = Domain.Enum.TransactionType.CancellationPayoutArtist;
                transactionAmount = appointment.DepositForApp * 0.5m;
                note = $"Giao dịch chi trả cho Artist do khách hàng hủy sát giờ - lịch hẹn #{appointment.Id}.";
            }
            // 4. Mở lại lịch của artist
            artistAvailability.Status = Domain.Enum.AvailabilityStatus.Available;

            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.ArtistAvailabilityRepository.Update(artistAvailability);

            if (appointment.VoucherId.HasValue)
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(appointment.VoucherId.Value);
                if (voucher != null && voucher.CurrentUsage.HasValue && voucher.CurrentUsage > 0)
                {
                    voucher.CurrentUsage -= 1;
                    _unitOfWork.VoucherRepository.Update(voucher);
                }
            }

            var userProgress = (await _unitOfWork.UserProgressRepository
                 .FindAsync(up => up.User.Id == appointment.CustomerId)).FirstOrDefault();
            userProgress.TotalCancellations += 1;
            _unitOfWork.UserProgressRepository.Update(userProgress);

            var transaction = new Domain.Entities.Transaction
            {
                AppointmentId = appointment.Id,
                UserId = appointment.CustomerId,
                ArtistId = appointment.ArtistMakeupId,
                Amount = transactionAmount,
                TransactionType = transactionType,
                TransactionStatus = Domain.Enum.TransactionStatus.Pending,
                PaymentProvider = "Bank Transfer",
                PaymentProviderTxnId = null,
                Note = note
            };

            _unitOfWork.TransactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = $"Lịch hẹn đã được {(appointment.Status == Domain.Enum.AppointmentStatus.WaitRefund ? "hủy và hoàn tiền" : "hủy mà không hoàn tiền")}.";

            return response;
        }
    }
}
