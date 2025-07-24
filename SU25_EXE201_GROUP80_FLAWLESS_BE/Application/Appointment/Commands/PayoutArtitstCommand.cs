using Application.Appointment.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class PayoutArtitstCommand : IRequest<PayoutArtitstResponse>
    {
        public Guid TransactionId { get; set; }
        public string TransactionCode { get; set; }
    }
}
