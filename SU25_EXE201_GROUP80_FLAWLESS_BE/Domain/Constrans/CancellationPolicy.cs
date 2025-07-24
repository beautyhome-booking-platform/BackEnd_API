using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constrans
{
    public static class CancellationPolicy
    {
        // Ví dụ: Hủy sớm trước 7 ngày
        public static readonly TimeSpan EarlyCancelTimeLimit = TimeSpan.FromDays(7);

        // Ví dụ: Hủy trễ trước 5 ngày
        public static readonly TimeSpan LateCancelTimeLimit = TimeSpan.FromDays(5);

        // Ví dụ: Không hoàn tiền nếu hủy trước 24 giờ
        public static readonly TimeSpan NoRefundTimeLimit = TimeSpan.FromHours(24);

        // Hoàn tiền 100% khi hủy sớm
        public const decimal EarlyCancelRefundPercentage = 1.0m;

        // Hoàn tiền 50% khi hủy trễ
        public const decimal LateCancelRefundPercentage = 0.5m;
    }
}
