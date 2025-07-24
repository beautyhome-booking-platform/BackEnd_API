using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum AvailabilityStatus
    {
        Available       = 0,   // Đang trống
        Booked       = 1,      // Đã được book
        Unavailable     = 2  // Tự đánh dấu không còn khả dụng (ví dụ tự hủy slot)
    }
}
