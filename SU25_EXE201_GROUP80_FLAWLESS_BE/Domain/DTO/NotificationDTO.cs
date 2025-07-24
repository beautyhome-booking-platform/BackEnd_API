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
    public class NotificationDTO
    {
        public Guid Id { get; set; }  // Khóa chính, định danh duy nhất cho thông báo
        public string Title { get; set; }  // Tiêu đề thông báo
        public string Content { get; set; }  // Nội dung chi tiết
        public NotificationType Type { get; set; }
        public string? Url { get; set; }  // Link nếu cần điều hướng (ví dụ dẫn tới đơn hàng/dịch vụ...)
        public bool IsRead { get; set; } = false;  // Đánh dấu đã đọc/chưa đọc
        public string UserId { get; set; }  // Navigation tới User
        public DateTime CreatedAt { get; set; }  // Ngày tạo thông báo, thường được gán tự động khi tạo mới
    }
}
