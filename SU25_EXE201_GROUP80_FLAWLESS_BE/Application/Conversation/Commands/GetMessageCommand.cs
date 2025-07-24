using Application.Conversation.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Commands
{
    public class GetMessageCommand : IRequest<GetMessageResponse>
    {
        public Guid ConversationId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
