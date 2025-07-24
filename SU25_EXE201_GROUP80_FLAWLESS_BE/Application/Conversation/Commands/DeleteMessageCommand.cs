using Application.Conversation.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Commands
{
    public class DeleteMessageCommand : IRequest<DeleteMessageResponse>
    {
        public Guid MessageId { get; set; }
    }
}
