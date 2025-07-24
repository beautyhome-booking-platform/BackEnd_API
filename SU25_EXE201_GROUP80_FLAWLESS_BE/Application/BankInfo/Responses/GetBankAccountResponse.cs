using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BankInfo.Responses
{
    public class GetBankAccountResponse : BaseResponse
    {
        public List<BankAccountDTO> BankAccount { get; set; }
    }
}
