using Application.ArtistAvailability.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Commands
{
    public class GetAvailabilityCommand : IRequest<GetAvailabilityResponse>
    {
        public string ArtistId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalTime { get; set; }
    }
}
