using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Responses
{
    public class RefreshTokenResponse : BaseResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
