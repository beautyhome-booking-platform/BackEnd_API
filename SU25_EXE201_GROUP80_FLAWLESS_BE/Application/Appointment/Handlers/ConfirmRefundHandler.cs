using Application.Appointment.Commands;
using Application.Appointment.Responses;
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
    public class ConfirmRefundHandler : IRequestHandler<ConfirmRefundCommand, ConfirmRefundResponse>
    {
        IUnitOfWork _unitOfWork;

        public ConfirmRefundHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ConfirmRefundResponse> Handle(ConfirmRefundCommand request, CancellationToken cancellationToken)
        {
            var response = new ConfirmRefundResponse();


            // 2. Find pending refund transaction
            var transaction = (await _unitOfWork.TransactionRepository
                .FindAsync(t => t.Id== request.TransactionId
                             && t.TransactionType == TransactionType.Refund
                             && t.TransactionStatus == TransactionStatus.Pending))
                .FirstOrDefault();


            if (transaction == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy giao dịch hoàn tiền đang chờ xử lý.";
                return response;
            }

            var appointment = (await _unitOfWork.AppointmentRepository.FindAsync(ap => ap.Id == (Guid)transaction.AppointmentId,
                                                                                q => q.Include(a => a.Customer))).FirstOrDefault();
             

            // 3. Mark transaction as completed
            transaction.TransactionStatus = TransactionStatus.Completed;

            // 4. Update appointment status to refunded
            appointment.Status = AppointmentStatus.Refunded;

            _unitOfWork.TransactionRepository.Update(transaction);
            _unitOfWork.AppointmentRepository.Update(appointment);

            var userProgress = (await _unitOfWork.UserProgressRepository
                .FindAsync(up => up.User.Id == appointment.CustomerId)).FirstOrDefault();

            userProgress.TotalSpent -= transaction.Amount; // Assuming this is the correct field to update

            _unitOfWork.UserProgressRepository.Update(userProgress);

            RefundReason refundReason;
            if (transaction.Note.Contains("Artist"))
            {
                refundReason = RefundReason.ArtistCancelled;
            }
            else
            {
                refundReason = RefundReason.CustomerCancelledEarly;
            }

            // 5. Create HistoryRefund
            var historyRefund = new Domain.Entities.HistoryRefund
            {
                AppointmentId = appointment.Id,
                Customer = appointment.Customer,
                TransactionId = transaction.Id,
                Transaction = transaction,
                RefundAmount = transaction.Amount,
                RefundReason = refundReason,
                Note = $"Hoàn tiền cho lịch hẹn #{appointment.Id} đã được admin xác nhận."
            };

           
            _unitOfWork.HistoryRefundRepository.AddAsync(historyRefund);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Xác nhận hoàn tiền thành công. Trạng thái đã được cập nhật.";

            return response;
        }
    }
}
