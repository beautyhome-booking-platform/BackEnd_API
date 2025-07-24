using Application.Notification.Responses;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Commands
{
    public class SendNotificationCommand : IRequest<SendNotificationResponse> 
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public string? Url { get; set; }
    }
}
