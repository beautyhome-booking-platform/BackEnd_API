using Application.Notification.Commands;
using Application.Notification.Responses;
using Infrastructure.ServiceHelp;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendNotificationCommand, SendNotificationResponse>
    {
        IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        public SendNotificationHandler(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<SendNotificationResponse> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new SendNotificationResponse();
            var user = (await _unitOfWork.UserAppRepository.FindAsync(x => x.Id == request.UserId)).FirstOrDefault();
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found";
                return response;
            }
            var notification = new Domain.Entities.Notification
            {
                Title = request.Title,
                Content = request.Content,
                Type = request.Type,
                Url = request.Url,
                User = user,
                IsRead = false
            };

            _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(request.UserId)
                  .SendAsync("ReceiveNotification", new
                  {
                      notification.Id,
                      notification.Title,
                      notification.Content,
                      notification.Type,
                      notification.Url,
                      notification.IsRead
                  });

            return response;
        }
    }
}
