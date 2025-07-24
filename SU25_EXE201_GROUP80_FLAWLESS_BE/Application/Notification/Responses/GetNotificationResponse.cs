using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Responses
{
    public class GetNotificationResponse : BaseResponse
    {
        public List<NotificationDTO> Notifications { get; set; } = new List<NotificationDTO>();
    }
}
