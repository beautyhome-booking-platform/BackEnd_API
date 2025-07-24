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
    public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackCommand, DeleteFeedbackResponse>
    {
        IUnitOfWork _unitOfWork;
        public DeleteFeedbackHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteFeedbackResponse> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteFeedbackResponse();

            var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.Id);
            if (feedback == null)
            {
                response.ErrorMessage = "Feedback không tồn tại";
                return response;
            }

            feedback.IsDeleted = true;
            _unitOfWork.FeedbackRepository.Update(feedback);
            response.IsSuccess = true;

            return response;
        }
    }
}
