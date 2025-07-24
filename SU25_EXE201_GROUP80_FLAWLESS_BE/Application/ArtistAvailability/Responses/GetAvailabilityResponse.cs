using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Responses
{
    public class GetAvailabilityResponse : BaseResponse
    {
        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }

    public class TimeSlot
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
