using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public string? PaymentProvider { get; set; }  // PayOS / BankTransfer

        public string? PaymentProviderTxnId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
