using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Responses
{
    public class ConfirmCompleteResponse : BaseResponse
    {
        public Guid AppointmentId { get; set; }
    }
}
