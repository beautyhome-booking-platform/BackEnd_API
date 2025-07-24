using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppointmentDetail : BaseEntity
    {
        [Required]
        public Guid AppointmentId { get; set; }  // FK đến Appointment

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }

        [Required]
        public Guid ServiceOptionId { get; set; }  // FK đến ServiceOption

        [ForeignKey(nameof(ServiceOptionId))]
        public ServiceOption ServiceOption { get; set; }

        // Optional: Số lượng (nếu có), hoặc ghi chú riêng cho lần đặt đó
        public int Quantity { get; set; } 

        [MaxLength(500)]
        public string? Note { get; set; }  // Ghi chú chi tiết cho từng option trong Appointment

        public decimal? UnitPrice { get; set; }  // Giá tại thời điểm đặt (để lưu lịch sử)

    }
}
