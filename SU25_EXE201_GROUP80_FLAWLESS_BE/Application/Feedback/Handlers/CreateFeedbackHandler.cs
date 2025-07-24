using Application.Feedback.Commands;
using Application.Feedback.Responses;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Handlers
{
    public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, CreateFeedbackResponse>
    {
        IUnitOfWork _unitOfWork;
        public CreateFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateFeedbackResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateFeedbackResponse();

            var appoinment = (await _unitOfWork.AppointmentRepository
                    .FindAsync(
                        a => a.Id == request.AppoinmentId,
                        query => query.Include(a => a.Customer)
                                      .Include(a => a.ArtistMakeup).Include(a => a.AppointmentsDetails)
                    )).FirstOrDefault();
            if (appoinment == null)
            {
                response.ErrorMessage = "Appointment not found";
                return response;
            }
            if(request.UserId != appoinment.CustomerId)
            {
                response.ErrorMessage = "User not a customer of this appoinment";
                return response;
            }
            if(appoinment.Status != AppointmentStatus.Completed)
            {
                response.ErrorMessage = "Appoinment not completed";
                return response;
            }
            var serviceOption = await _unitOfWork.ServiceOptionRepository.GetByIdAsync(request.ServiceOptionId);
            if (serviceOption == null)
            {
                response.ErrorMessage = "Service Option not found";
                return response;
            }

            var feedback = new Domain.Entities.Feedback
            {
                AppoinmentId = request.AppoinmentId,
                Appointment = appoinment,
                Content = request.Content,
                Rating = request.Rating,
                ServiceOption = serviceOption,
                ServiceOptionId = request.ServiceOptionId,
                User = appoinment.Customer
            };

            if (feedback != null)
            {
                _unitOfWork.FeedbackRepository.AddAsync(feedback);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccess = true;
                response.Feedback = new Domain.DTO.FeedbackDTO
                {
                    Id = feedback.Id,
                    AppoinmentId = feedback.AppoinmentId,
                    UserId = feedback.User.Id,
                    Content = feedback.Content,
                    Rating = feedback.Rating,
                    ServiceOptionId = feedback.ServiceOptionId,
                    UserName = feedback.User.Name
                };
            }
            return response;
        }
    }
}
