using Application.Appointment.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class ConfirmCompleteCommand : IRequest<ConfirmCompleteResponse>
    {
        public Guid AppointmentId { get; set; }
    }
}
