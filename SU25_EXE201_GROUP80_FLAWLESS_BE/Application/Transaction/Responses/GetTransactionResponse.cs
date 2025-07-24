using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transaction.Responses
{
    public class GetTransactionResponse : BaseResponse
    {
        public List<TransactionDTO> TransactionDTOs { get; set; }
    }
}
