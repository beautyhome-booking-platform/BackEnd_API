using Application.Feedback.Commands;
using Application.Feedback.Responses;
using Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Handlers
{
    public class GetFeedbackHandler : IRequestHandler<GetFeedbackCommand, GetFeedbackResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetFeedbackResponse> Handle(GetFeedbackCommand request, CancellationToken cancellationToken)
        {
            var response = new GetFeedbackResponse();
            List<Guid>? artistServiceOptionIds = null;

            if (!string.IsNullOrEmpty(request.ArtistId))
            {
                artistServiceOptionIds = (await _unitOfWork.ServiceOptionRepository
                        .FindAsync(so => so.ArtistId == request.ArtistId && !so.IsDeleted))
                        .Select(so => so.Id)
                        .ToList();

                
                if (artistServiceOptionIds == null || !artistServiceOptionIds.Any())
                {
                    response.ErrorMessage = "Artist does not have any associated ServiceOptions.";
                    return response;
                }
            }


            var feedback = await _unitOfWork.FeedbackRepository
                    .FindAsync(fb =>
                        !fb.IsDeleted &&
                        (request.Id == null || fb.Id == request.Id) &&
                        (string.IsNullOrEmpty(request.UserId) || fb.User.Id == request.UserId) &&
                        (request.ServiceOptionId == null || fb.ServiceOptionId == request.ServiceOptionId) &&
                        (request.AppoinmentId == null || fb.AppoinmentId == request.AppoinmentId) &&
                        (string.IsNullOrEmpty(request.ArtistId) || artistServiceOptionIds!.Contains(fb.ServiceOptionId)),
                        query => query.Include(fb => fb.ServiceOption)
                                      .Include(fb => fb.User)
                                      .Include(fb => fb.Appointment)
                                                .ThenInclude(a => a.ArtistMakeup)
                    );
            if (feedback.Any())
            {
                var feedbackDTOs = feedback.Select(f => new FeedbackDTO
                {
                    Id = f.Id,
                    UserId = f.User.Id,
                    AppoinmentId = f.AppoinmentId,
                    ServiceOptionId = f.ServiceOptionId,
                    Content = f.Content,
                    Rating = f.Rating,
                    UserName = f.User.Name,
                }).ToList();

                response.feedbackDTOs = feedbackDTOs;
                response.IsSuccess = true;
            }
            else
            {
                response.ErrorMessage = "Feedback not found or not have any feedback.";
            }
            return response;
        }
    }
}
