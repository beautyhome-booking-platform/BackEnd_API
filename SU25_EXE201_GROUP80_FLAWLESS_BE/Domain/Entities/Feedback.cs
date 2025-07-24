using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Feedback : BaseEntity
    {
        [Required]
        [ForeignKey("UserId")]
        public UserApp User { get; set; }

        public Guid ServiceOptionId { get; set; }
        [ForeignKey(nameof(ServiceOptionId))]
        public ServiceOption ServiceOption { get; set; }

        public Guid AppoinmentId { get; set; }
        [ForeignKey(nameof(AppoinmentId))]
        public Appointment Appointment { get; set; }

        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
