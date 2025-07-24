using Application.UserService.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Commands
{
    public class AskChatCommand : IRequest<AskChatResponse>
    {
        public string Message { get; set; }
    }
}
