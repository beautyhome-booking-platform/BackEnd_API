using Application.BankInfo.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BankInfo.Commands
{
    public  class DeleteBankAccountCommand : IRequest<DeleteBankAccountResponse>
    {
        public Guid Id { get; set; }
    }
}
