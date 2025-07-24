using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Area : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string City { get; set; }  // Ví dụ: TP.HCM

        [Required]
        [MaxLength(100)]
        public string District { get; set; }  // Ví dụ: Quận 1

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
