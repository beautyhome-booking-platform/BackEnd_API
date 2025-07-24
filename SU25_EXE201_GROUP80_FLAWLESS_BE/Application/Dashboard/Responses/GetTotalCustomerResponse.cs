using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class GetTotalCustomerResponse : BaseResponse
    {
        public int TotalCustomerAllYear { get; set; }
        public List<TotalCustomerPerYearDto> PerYear { get; set; } = new();
    }
    public class TotalCustomerPerYearDto
    {
        public int Year { get; set; }
        public int TotalPerYear { get; set; }
        public Dictionary<string, TotalCustomerMonthDto> Months { get; set; } = new();
    }

    public class TotalCustomerMonthDto
    {
        public int TotalCustomer { get; set; }
        public int NewCustomer { get; set; }
        public int ReturnCustomer { get; set; }
        public int CancelCustomer { get; set; }
    }
}
