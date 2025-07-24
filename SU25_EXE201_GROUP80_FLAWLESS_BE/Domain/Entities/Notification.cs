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
    public class Notification : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }  // Tiêu đề thông báo

        [Required]
        public string Content { get; set; }  // Nội dung chi tiết

        [Required]
        public NotificationType Type { get; set; }

        public string? Url { get; set; }  // Link nếu cần điều hướng (ví dụ dẫn tới đơn hàng/dịch vụ...)

        public bool IsRead { get; set; } = false;  // Đánh dấu đã đọc/chưa đọc

        [Required]

        [ForeignKey("UserId")]
        public UserApp User { get; set; }  // Navigation tới User

    }
}
