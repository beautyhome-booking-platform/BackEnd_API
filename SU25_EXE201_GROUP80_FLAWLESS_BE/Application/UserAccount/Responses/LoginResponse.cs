using Azure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Responses
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool RequiresTwoFactor { get; set; } = false;
        public string Message { get; set; }
    }
}
