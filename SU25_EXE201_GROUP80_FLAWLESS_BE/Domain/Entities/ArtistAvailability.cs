using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class ArtistAvailability : BaseEntity
    {
        [Required]

        [ForeignKey("ArtistId")]
        public UserApp Artist { get; set; }

        [Required]
        public DateTime InvailableDateStart { get; set; }

        [Required]
        public DateTime InvailableDateEnd { get; set; }

        [MaxLength(200)]
        public string? Note { get; set; }            // Ghi chú (nếu cần: ví dụ "Ngày lễ có thể tăng phí")

        public AvailabilityStatus Status { get; set; } = AvailabilityStatus.Available;
    }
}
