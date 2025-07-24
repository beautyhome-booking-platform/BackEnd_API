using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Responses
{
    public class GetScheduleResponse : BaseResponse
    {
        public List<Time> Schedule { get; set; } = new List<Time>();
    }
    public class Time
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AvailabilityStatus Status { get; set; } 
    }
}
