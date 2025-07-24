using Application.Feedback.Commands;
using Application.Feedback.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Handlers
{
    public class UpdateFeedbackHandler : IRequestHandler<UpdateFeedbackCommand, UpdateFeedbackResponse>
    {
        IUnitOfWork _unitOfWork;
        public UpdateFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateFeedbackResponse> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateFeedbackResponse();

            var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.Id);
            if (feedback == null)
            {
                response.ErrorMessage = "Feedback không tồn tại";
                return response;
            }

            if(!string.IsNullOrEmpty(request.Content))
                feedback.Content = request.Content;
            if (request.Rating != null)
                feedback.Rating = request.Rating;

            _unitOfWork.FeedbackRepository.Update(feedback);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;

            return response;
        }
    }
}
