using Application.ArtistAvailability.Responses;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Commands
{
    public class UpdateAvailabilityCommand : IRequest<UpdateAvailabilityResponse>
    {
        public string ArtistId { get; set; }

        public List<TimeRequest> TimeRequests { get; set; } = new List<TimeRequest>();
    }
    public class TimeRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AvailabilityStatus Status { get; set; }
    }
}
