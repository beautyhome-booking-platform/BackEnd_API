using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class TotalBookingResponse : BaseResponse
    {
        public int TotalBookingAllYear { get; set; }
        public List<BookingPerYearDto> PerYear { get; set; } = new();
    }
    public class BookingPerYearDto
    {
        public int Year { get; set; }
        public int TotalBookingPerYear { get; set; }
        public MonthBookingDto BestBooking { get; set; }
        public MonthCancelDto BestCancel { get; set; }
        public Dictionary<string, int> Booking { get; set; } = new();
        public Dictionary<string, int> Cancel { get; set; } = new();
    }

    public class MonthBookingDto
    {
        public string Month { get; set; }
        public int Booking { get; set; }
    }

    public class MonthCancelDto
    {
        public string Month { get; set; }
        public int Cancel { get; set; }
    }
}
