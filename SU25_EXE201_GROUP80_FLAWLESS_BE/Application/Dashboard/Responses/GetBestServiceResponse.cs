using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class GetBestServiceResponse : BaseResponse
    {
        public List<BestServiceYearDto> BestService { get; set; } = new();
    }
    public class BestServiceYearDto
    {
        public int Year { get; set; }
        public Dictionary<string, List<BestServiceMonthDto>> Months { get; set; } = new();
    }

    public class BestServiceMonthDto
    {
        public Guid IdSer { get; set; }
        public string NameSer { get; set; }
        public double AvgRating { get; set; }
    }
}
