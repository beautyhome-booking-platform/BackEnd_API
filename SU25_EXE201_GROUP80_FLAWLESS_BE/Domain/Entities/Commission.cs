using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Commission : BaseEntity
    {
        public Guid? AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment? Appointment { get; set; }

        public CommissionType CommissionType { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
    }
}
