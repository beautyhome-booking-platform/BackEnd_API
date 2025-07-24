using Application.UserAccount.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class LoginGoogleCommand : IRequest<LoginGoogleResponse>
    {
        public string TokenId { get; set; } // nhận từ client (Google Identity Token)
    }
}
