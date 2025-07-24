using Application.Appointment.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class ArtistCancelledBookingCommand : IRequest<ArtistCancelledBookingResponse>
    {
        public Guid AppoinmentId { get; set; }
    }
}
