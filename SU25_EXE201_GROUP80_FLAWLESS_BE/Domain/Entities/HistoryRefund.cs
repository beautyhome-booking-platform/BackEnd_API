using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public  class HistoryRefund : BaseEntity
    {
        [Required]
        public Guid AppointmentId { get; set; }  // Liên kết với lịch hẹn

        [ForeignKey(nameof(AppointmentId))]
        public  Appointment Appointment { get; set; }

        [Required]

        [ForeignKey("CustomerId")]
        public  UserApp Customer { get; set; }

        [Required]
        public Guid TransactionId { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public  Transaction Transaction { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }  // Số tiền hoàn lại

        [Required]
        public RefundReason RefundReason { get; set; }  // Lý do hoàn tiền (enum)

        [MaxLength(500)]
        public string? Note { get; set; }  // Ghi chú chi tiết (ví dụ: "Hủy sát giờ, hoàn 50%")

    }
}
