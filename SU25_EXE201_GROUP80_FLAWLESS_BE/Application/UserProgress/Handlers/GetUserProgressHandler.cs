using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class GetUserProgressHandler : IRequestHandler<GetUserProgressCommand, GetUserProgressResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetUserProgressHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetUserProgressResponse> Handle(GetUserProgressCommand request, CancellationToken cancellationToken)
        {
            var response = new GetUserProgressResponse();

            var userProgress = (await _unitOfWork.UserProgressRepository
                .FindAsync(up => (up.User.Id == request.UserId)))
                .FirstOrDefault();
            if (userProgress == null)
            {
                response.ErrorMessage = "User not found";
                return response;
            }

            response.UserProgress = new UserProgressDTO
            {
                Id = userProgress.Id,
                Points = userProgress.Points,
                TotalCompletedAppointments = userProgress.TotalCompletedAppointments,
                TotalCancellations = userProgress.TotalCancellations,
                TotalSpent = userProgress.TotalSpent,
                LastRankUpgradeDate = userProgress.LastRankUpgradeDate,
                Note = userProgress.Note,
                IsPremium = userProgress.IsPremium,
                UserTier = userProgress.UserTier,
            };
            return response;
        }
    }
}
