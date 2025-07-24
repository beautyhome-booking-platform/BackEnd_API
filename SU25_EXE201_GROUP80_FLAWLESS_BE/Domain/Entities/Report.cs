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
    public class Report : BaseEntity
    {
        [Required]
        public string ReporterId { get; set; }

        [ForeignKey(nameof(ReporterId))] // Người báo cáo
        public  UserApp Reporter { get; set; }

        [Required]
        public string ReportedUserId { get; set; }

        [ForeignKey(nameof(ReportedUserId))]  // Người bị báo cáo
        public  UserApp ReportedUser { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }   // Lý do tự do (có thể viết thêm)

        [Required]
        public ReportReason ReportReason { get; set; } // Enum lý do báo cáo

        public Guid? AppointmentId { get; set; }  // Nếu báo cáo liên quan đến lịch hẹn

        [ForeignKey(nameof(AppointmentId))]
        public  Appointment? Appointment { get; set; }

        public bool IsResolved { get; set; } = false; // Đánh dấu đã xử lý
    }
}
