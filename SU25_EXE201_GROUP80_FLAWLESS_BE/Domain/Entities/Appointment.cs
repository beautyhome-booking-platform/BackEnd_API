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
    public class Appointment : BaseEntity
    {
        [Required]
        public string CustomerId { get; set; }    // Khách hàng (khóa ngoại)

        [ForeignKey(nameof(CustomerId))]
        public virtual UserApp Customer { get; set; }   // Liên kết với Customer

        [Required]
        public string ArtistMakeupId { get; set; }     // Nghệ sĩ makeup (khóa ngoại)

        [ForeignKey(nameof(ArtistMakeupId))]
        public virtual UserApp ArtistMakeup { get; set; } // Liên kết với ArtistMakeup

        public DateTime AppointmentDate { get; set; }   // Thời gian hẹn

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }    // Địa chỉ thực hiện

        [MaxLength(200)]
        public string? Note { get; set; }       // Ghi chú thêm

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public Guid? VoucherId { get; set; }    // Nếu có dùng voucher

        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }// Tổng tiền đã giảm (nếu có voucher)
        public decimal TotalAmountAfterDiscount { get; set; } // Tổng tiền sau khi giảm (nếu có voucher)
        public decimal DepositForApp { get; set; } // Tiền cọc (nếu có, nếu không thì = 0)
        public decimal AmountToPayForArtist { get; set; } // Tổng tiền cần thanh toán (sau giảm giá, nếu có voucher)

        public ICollection<AppointmentDetail>? AppointmentsDetails { get; set; } = new List<AppointmentDetail>();
    }
}
