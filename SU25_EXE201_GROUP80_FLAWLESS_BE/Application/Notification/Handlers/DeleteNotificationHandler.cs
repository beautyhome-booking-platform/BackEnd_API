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
    public class DeleteNotificationHandler : IRequestHandler<DeleteNotificationCommand, DeleteNotificationResponse>
    {
        IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        public DeleteNotificationHandler(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<DeleteNotificationResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteNotificationResponse();

            var notification = (await _unitOfWork.NotificationRepository.FindAsync(n => n.Id == request.Id && !n.IsDeleted)).FirstOrDefault();
            if (notification == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Notification not found";
                return response;
            }
            notification.IsDeleted = true;
            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(notification.User.Id.ToString())
            .SendAsync("NotificationDelete", new
            {
                Id = notification.Id
            });

            response.IsSuccess = true;

            return response;
        }
    }
}
