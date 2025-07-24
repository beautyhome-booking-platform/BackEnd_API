using Application.Transaction.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Transaction.Commands
{
    public class GetTransactionCommand : IRequest<GetTransactionResponse>
    {
        public Guid? Id { get; set; }
        public Guid? AppointmentId { get; set; }
        public string? UserId { get; set; }
        public TransactionStatus? Status { get; set; }
    }
}
