using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Service : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public Guid ServiceCategoryId { get; set; }
        [ForeignKey(nameof(ServiceCategoryId))]
        public  ServiceCategories? ServiceCategory { get; set; }

        public  ICollection<ServiceOption> ServiceOptions { get; set; } = new List<ServiceOption>();

        public ICollection<InformationArtist> InterestedArtists { get; set; } = new List<InformationArtist>();
    }
}
