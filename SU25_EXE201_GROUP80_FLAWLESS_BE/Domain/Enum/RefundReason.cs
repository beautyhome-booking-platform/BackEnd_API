using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum RefundReason
    {
        CustomerBookingRejectedByArtist = 0,    // Người dùng đặt lịch nhưng bị artist từ chối
        ArtistCancelled                 = 1,                    // Artist hủy lịch
        CustomerCancelledEarly          = 2,             // Người dùng hủy lịch sớm (hoàn 100%)
        CustomerCancelledModerate       = 3,         // Người dùng hủy lịch trễ (hoàn 50%)
        CustomerCancelledLate           = 4,              // Người dùng hủy cận giờ (không hoàn hoặc chia tiền)
        ConflictOccurred                = 5,              // Xảy ra xung đột (lý do khác)
        ServiceNotSatisfactory          = 6              // Dịch vụ không đảm bảo chất lượng
    }
}
