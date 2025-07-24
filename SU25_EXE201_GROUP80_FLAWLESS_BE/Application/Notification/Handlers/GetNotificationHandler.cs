using Application.Notification.Commands;
using Application.Notification.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Handlers
{
    public class GetNotificationHandler : IRequestHandler<GetNotificationCommand, GetNotificationResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetNotificationResponse> Handle(GetNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new GetNotificationResponse();

            var notifis = await _unitOfWork.NotificationRepository.FindAsync( n => 
                                                                            (n.User.Id == request.UserId || string.IsNullOrEmpty(request.UserId)) &&
                                                                            (n.Id == request.Id || request.Id == null) &&
                                                                            (!n.IsDeleted)
                                                                             );

            response.Notifications = notifis.Select(n => new Domain.DTO.NotificationDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Type = n.Type,
                Url = n.Url,
                IsRead = n.IsRead,
                CreatedAt = (DateTime)n.CreateAt
            }).ToList();
            response.IsSuccess = true;

            return response;
        }
    }
}
