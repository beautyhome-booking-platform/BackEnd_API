using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceOption : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public int      Duration     { get; set; }
        public int? DiscountPercent { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        
        public string? ArtistId { get; set; }

        [ForeignKey(nameof(Service))]
        public Guid ServiceId { get; set; }
        public  Service? Service { get; set; }
    }
}
