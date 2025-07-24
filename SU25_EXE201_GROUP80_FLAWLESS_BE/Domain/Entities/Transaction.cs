using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid? AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public  Appointment? Appointment { get; set; }

        public string UserId  { get; set; }

        public string ArtistId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public TransactionStatus TransactionStatus { get; set; }

        [MaxLength(100)]
        public string? PaymentProvider { get; set; }  // PayOS / BankTransfer

        [MaxLength(100)]
        public string? PaymentProviderTxnId { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

    }
}
