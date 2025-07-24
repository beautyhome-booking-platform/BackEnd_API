using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum NotificationType
    {
        System      = 0,           // Thông báo hệ thống (chung)
        Booking     = 1,          // Đặt lịch/dịch vụ
        Promotion   = 2,        // Khuyến mãi
        Reminder    = 3,         // Nhắc nhở
        Payment     = 4,          // Thanh toán  
    }
}
