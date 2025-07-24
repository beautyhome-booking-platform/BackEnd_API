using Application.BankInfo.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BankInfo.Commands
{
    public class AddBackAccountCommand : IRequest<AddBackAccountResponse>
    { 
        public string UserId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
    }
}
