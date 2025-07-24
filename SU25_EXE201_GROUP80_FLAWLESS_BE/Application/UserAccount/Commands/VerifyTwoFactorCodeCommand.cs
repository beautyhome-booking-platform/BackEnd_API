using Application.UserAccount.Handlers;
using Application.UserAccount.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class VerifyTwoFactorCodeCommand : IRequest<VerifyTwoFactorCodeResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string TwoFactorCode { get; set; } = string.Empty;
    }
}
