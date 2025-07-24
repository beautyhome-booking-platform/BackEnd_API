using Application.Notification.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Commands
{
    public class GetNotificationCommand : IRequest<GetNotificationResponse>
    {
        public Guid? Id { get; set; }
        public string? UserId { get; set; }
    }
}
