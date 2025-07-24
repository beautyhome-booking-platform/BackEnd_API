using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class GetBestArtistResponse : BaseResponse
    {
        public List<BestArtistYearDto> BestArtist { get; set; } = new();
    }
    public class BestArtistYearDto
    {
        public int Year { get; set; }
        public Dictionary<string, List<BestArtistMonthDto>> Months { get; set; } = new();
    }

    public class BestArtistMonthDto
    {
        public string IdAr { get; set; }
        public string NameAr { get; set; }
        public string AvatarAr { get; set; }
        public int Status { get; set; }
        public double AvgRating { get; set; }
        public decimal TotalRevenueInMonth { get; set; }
        public int TotalBookingInMonth { get; set; }
        public int TotalCancelInMonth { get; set; }
    }
}
