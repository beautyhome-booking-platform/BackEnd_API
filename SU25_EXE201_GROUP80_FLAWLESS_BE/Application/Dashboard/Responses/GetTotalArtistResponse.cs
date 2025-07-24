using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class GetTotalArtistResponse : BaseResponse
    {
        public int TotalArtistAllYear { get; set; }
        public List<TotalArtistPerYearDto> PerYear { get; set; } = new();
    }
    public class TotalArtistPerYearDto
    {
        public int Year { get; set; }
        public int TotalArtistPerYear { get; set; }
        public Dictionary<string, TotalArtistMonthDto> Months { get; set; } = new();
    }

    public class TotalArtistMonthDto
    {
        public int TotalArtist { get; set; }
        public int NewArtist { get; set; }
        public int BanArtist { get; set; }
    }
}
