using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum MessageStatus
    {
        Sent        = 0,        // Tin nhắn đã gửi
        Delivered   = 1,   // Tin nhắn đã giao
        Read        = 2,        // Tin nhắn đã đọc
        Deleted     = 3      // Tin nhắn đã xóa
    }
}
