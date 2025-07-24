using Application.Appointment.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class CancelledBookingCommand : IRequest<CancelledBookingResponse>
    {
        public Guid AppointmentId { get; set; } // thêm field AppointmentId từ client
    }
}
