using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Responses
{
    public class GetListChatResponse : BaseResponse
    {
        public List<ChatConversationDto> Conversations { get; set; } = new();
    }

    public class ChatConversationDto
    {
        public Guid ConversationId { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerAvatar { get; set; }
        public string LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public bool IsArchived { get; set; }
    }
}
