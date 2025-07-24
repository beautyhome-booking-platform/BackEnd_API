using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum AppointmentStatus
    {
        Pending     = 0,      // Chờ xác nhận
        Confirmed   = 1,    // Đã xác nhận
        Completed   = 2,    // Đã hoàn thành
        Canceled    = 3,     // Đã hủy
        Rejected    = 4,
        WaitRefund  = 5,
        Refunded    = 6,
    }
}
