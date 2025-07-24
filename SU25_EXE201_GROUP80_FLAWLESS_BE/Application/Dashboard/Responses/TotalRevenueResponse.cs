using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Responses
{
    public class TotalRevenueResponse : BaseResponse
    {
        public decimal TotalIncomeAllYear { get; set; }
        public List<RevenuePerYearDto> PerYear { get; set; } = new();
    }
    public class RevenuePerYearDto
    {
        public int Year { get; set; }
        public decimal TotalIncomePerYear { get; set; }
        public decimal TotalNetProfitPerYear { get; set; }
        public decimal TotalRefundPerYear { get; set; }
        public decimal TotalPayoutForArtist { get; set; }

        public MonthValueDto BestIncome { get; set; }
        public MonthValueDto BestNetProfit { get; set; }
        public MonthValueDto BestRefund { get; set; }

        public Dictionary<string, decimal> Income { get; set; } = new();
        public Dictionary<string, decimal> Refund { get; set; } = new();
        public Dictionary<string, decimal> NetProfit { get; set; } = new();
    }

    public class MonthValueDto
    {
        public string Month { get; set; }
        public decimal Value { get; set; }
    }
}
