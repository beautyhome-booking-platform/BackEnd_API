using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Responses
{
    public class SendMessageResponse : BaseResponse
    {
        public MessageDto Message { get; set; }
    }

    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public Guid ConversationId { get; set; }
        public bool IsRead { get; set; }
    }
}
