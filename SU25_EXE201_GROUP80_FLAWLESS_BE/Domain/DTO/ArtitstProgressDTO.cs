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
    public class ArtitstProgressDTO
    {
        public Guid Id { get; set; }
        public int Points { get; set; } = 0;   // Điểm tích lũy (nếu cần)

        public ArtistRank Rank { get; set; } = ArtistRank.Begginer;  // Hạng của nghệ sĩ (ví dụ: Beginner, Intermediate, Advanced)

        public int TotalCompletedAppointments { get; set; } = 0;     // Tổng số lịch hẹn hoàn thành

        public int TotalCancellations { get; set; } = 0;            // Tổng số lần hủy

        public decimal TotalReceive { get; set; } = 0;                // Tổng số tiền đã chi tiêu

        public DateTime? LastRankUpgradeDate { get; set; }  // Ngày thay đổi hạng gần nhất

        public RequestStatus Note { get; set; } = RequestStatus.Requested;

        public bool IsActive { get; set; } = false;  // Đánh dấu xem hạng có còn hiệu lực hay không
    }
}
