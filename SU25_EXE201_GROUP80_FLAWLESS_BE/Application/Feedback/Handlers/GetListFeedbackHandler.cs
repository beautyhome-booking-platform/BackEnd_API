using Application.Feedback.Commands;
using Application.Feedback.Responses;
using Domain.DTO;
using Domain.Entities;
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
    public class GetListFeedbackHandler : IRequestHandler<GetListFeedbackCommand, GetListFeedbackResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetListFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetListFeedbackResponse> Handle(GetListFeedbackCommand request, CancellationToken cancellationToken)
        {
            var response = new GetListFeedbackResponse();

            
            var feedbacks = await _unitOfWork.FeedbackRepository.FindAsync(
                    expression: null,
                    include: query => query
                        .Include(f => f.User)
                        .Include(so => so.ServiceOption)
                        .Include(f => f.Appointment)
                            .ThenInclude(a => a.ArtistMakeup)
                    );

            foreach (var feedback in feedbacks)
            {
                if (!feedback.IsDeleted)
                {
                    response.Feedbacks.Add(new FeedbackListDTO
                    {
                        Id = feedback.Id,
                        Customer = new Customerdto
                        {
                            Id = feedback.User?.Id,
                            Name = feedback.User?.Name,
                            ImageUrl = feedback.User?.ImageUrl
                        },
                        Artist = new Artistdto
                        {
                            Id = feedback.Appointment.ArtistMakeupId,
                            Name = feedback.Appointment.ArtistMakeup.Name,
                        },
                        ServiceOption = new ServiceOptiondto
                        {
                            Id = feedback.ServiceOptionId,
                            Name = feedback.ServiceOption.Name,
                        },
                        Content = feedback.Content,
                        Rating = feedback.Rating,
                        DateTime = feedback.CreateAt,
                    });
                }
            }

            var count = response.Feedbacks.Count();
            response.IsSuccess = true;
            response.Message = $"Get feedback list successfully! Total: {response.Feedbacks.Count} entries.";
            return response;
        }
    }
}
