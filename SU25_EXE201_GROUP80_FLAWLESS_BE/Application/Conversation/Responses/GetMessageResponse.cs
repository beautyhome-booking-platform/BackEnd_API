using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Responses
{
    public class GetMessageResponse : BaseResponse
    {
        public List<MessageDTo> Messages { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class MessageDTo
    {
        public Guid MessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public MessageStatus Status { get; set; }
    }
}
