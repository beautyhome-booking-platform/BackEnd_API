using Application.Appointment.Commands;
using Application.Appointment.Responses;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Handlers
{
    public class ConfirmCompleteHandler : IRequestHandler<ConfirmCompleteCommand, ConfirmCompleteResponse>
    {
        IUnitOfWork _unitOfWork;

        public ConfirmCompleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ConfirmCompleteResponse> Handle(ConfirmCompleteCommand request, CancellationToken cancellationToken)
        {
            var response = new ConfirmCompleteResponse();

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                response.ErrorMessage = "Appoinment is not exist";
                return response;
            }
            if(appointment.Status != AppointmentStatus.Confirmed)
            {
                response.ErrorMessage = "Appoinment status is not suitable to active";
                return response;
            }

            appointment.Status = AppointmentStatus.Completed;
            _unitOfWork.AppointmentRepository.Update(appointment);

            var commission = new Commission
            {
                AppointmentId = appointment.Id,
                CommissionType = CommissionType.CommissionAppointmentComplete,
                Rate = 0.20m, // Assuming this is the completion rate
                Amount = appointment.DepositForApp
            };

            _unitOfWork.CommissionRepository.AddAsync(commission);

            var userProgress = (await _unitOfWork.UserProgressRepository.FindAsync(up => up.User.Id == appointment.CustomerId)).FirstOrDefault();

            var artistProgress = (await _unitOfWork.ArtistProgressRepository.FindAsync(ap => ap.Artist.Id == appointment.ArtistMakeupId)).FirstOrDefault();

            // Update user progress
            userProgress.TotalCompletedAppointments += 1;
            userProgress.TotalSpent += appointment.TotalAmountAfterDiscount; // or correct field
                                                                             // Update points/tiers/rank as needed
            _unitOfWork.UserProgressRepository.Update(userProgress);
            // Update artist progress
            artistProgress.TotalCompletedAppointments += 1;
            artistProgress.TotalReceive += appointment.AmountToPayForArtist; // or correct field

            _unitOfWork.ArtistProgressRepository.Update(artistProgress);

            await _unitOfWork.SaveChangesAsync();

            response.AppointmentId = appointment.Id;
            response.IsSuccess = true;

            return response;
        }
    }
}
