using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserProgressDTO
    {
        public Guid Id { get; set; }

        public UserTier UserTier { get; set; } = UserTier.Basic; // Hạng người dùng

        public bool IsPremium { get; set; } = false;

        public int TotalCompletedAppointments { get; set; } = 0;     // Tổng số lịch hẹn hoàn thành

        public int TotalCancellations { get; set; } = 0;            // Tổng số lần hủy

        public decimal TotalSpent { get; set; } = 0;                // Tổng số tiền đã chi tiêu

        public int Points { get; set; } = 0;                        // Điểm tích lũy (nếu cần)

        public DateTime? LastRankUpgradeDate { get; set; }          // Ngày nâng hạng gần nhất

        public string? Note { get; set; }                           // Ghi chú thêm (nếu cần)
    }
}
