using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Responses
{
    public class LoginGoogleResponse : BaseResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; } 
        public string RefreshToken { get; set; }
    }
}

