using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArtistProgress : BaseEntity
    {
        [Required]
        [ForeignKey("ArtistId")]
        public UserApp Artist { get; set; }

        public int Points { get; set; } = 0;   // Điểm tích lũy (nếu cần)

        [Required]
        [MaxLength(100)]
        public ArtistRank Rank { get; set; } = ArtistRank.Begginer;  // Hạng của nghệ sĩ (ví dụ: Beginner, Intermediate, Advanced)

        public int TotalCompletedAppointments { get; set; } = 0;     // Tổng số lịch hẹn hoàn thành

        public int TotalCancellations { get; set; } = 0;            // Tổng số lần hủy

        public decimal TotalReceive { get; set; } = 0;               

        public DateTime? LastRankUpgradeDate { get; set; }  // Ngày thay đổi hạng gần nhất

        [MaxLength(500)]
        public RequestStatus Note { get; set; } = RequestStatus.Requested;

        public bool IsActive { get; set; } = false;  // Đánh dấu xem hạng có còn hiệu lực hay không

        public InformationArtist InformationArtist { get; set; }

    }
}
