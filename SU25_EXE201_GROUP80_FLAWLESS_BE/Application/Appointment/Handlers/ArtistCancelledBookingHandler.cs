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
    public class ArtistCancelledBookingHandler : IRequestHandler<ArtistCancelledBookingCommand, ArtistCancelledBookingResponse>
    {
        IUnitOfWork _unitOfWork;
        public ArtistCancelledBookingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ArtistCancelledBookingResponse> Handle(ArtistCancelledBookingCommand request, CancellationToken cancellationToken)
        {
            var response = new ArtistCancelledBookingResponse();

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(request.AppoinmentId);
            if (appointment == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy lịch hẹn.";
                return response;
            }

            var artistAvailability = (await _unitOfWork.ArtistAvailabilityRepository
                .FindAsync(a => a.Note == request.AppoinmentId.ToString()))
                .FirstOrDefault();

            if (artistAvailability == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thời gian làm việc tương ứng.";
                return response;
            }

            // Update statuses
            appointment.Status = Domain.Enum.AppointmentStatus.Rejected;
            artistAvailability.Status = Domain.Enum.AvailabilityStatus.Available;

            if (appointment.VoucherId.HasValue)
            {
                var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(appointment.VoucherId.Value);
                if (voucher != null && voucher.CurrentUsage.HasValue && voucher.CurrentUsage > 0)
                {
                    voucher.CurrentUsage -= 1;
                    _unitOfWork.VoucherRepository.Update(voucher);
                }
            }

            var artistProgress = (await _unitOfWork.ArtistProgressRepository.FindAsync(ap => ap.Artist.Id == appointment.ArtistMakeupId)).FirstOrDefault();
            artistProgress.TotalCancellations += 1;
            _unitOfWork.ArtistProgressRepository.Update(artistProgress);

            // Create refund transaction
            var transaction = new Domain.Entities.Transaction
            {
                AppointmentId = appointment.Id,
                UserId = appointment.CustomerId,
                ArtistId = appointment.ArtistMakeupId,
                Amount = appointment.DepositForApp,
                TransactionType = Domain.Enum.TransactionType.Refund,
                TransactionStatus = Domain.Enum.TransactionStatus.Pending,
                PaymentProvider = "Bank Transfer",
                PaymentProviderTxnId = null,
                Note = $"Hoàn tiền đầy đủ cho khách hàng do Artist hủy lịch hẹn #{appointment.Id}"
            };

            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.ArtistAvailabilityRepository.Update(artistAvailability);
            _unitOfWork.TransactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Lịch hẹn đã được Artist hủy. Hệ thống sẽ hoàn toàn bộ tiền cọc cho khách hàng.";
            return response;
        }
    }
}
