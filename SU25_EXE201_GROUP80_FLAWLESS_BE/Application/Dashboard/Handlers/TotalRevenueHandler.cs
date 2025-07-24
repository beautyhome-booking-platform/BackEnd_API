using Application.Dashboard.Commands;
using Application.Dashboard.Responses;
using Domain.Enum;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Handlers
{
    public class TotalRevenueHandler : IRequestHandler<TotalRevenueCommand, TotalRevenueResponse>
    {
        IUnitOfWork _unitOfWork;
        public TotalRevenueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TotalRevenueResponse> Handle(TotalRevenueCommand request, CancellationToken cancellationToken)
        {
            var response = new TotalRevenueResponse { IsSuccess = true };

            // Lấy dữ liệu
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            var commissions = await _unitOfWork.CommissionRepository.GetAllAsync();
            var refunds = await _unitOfWork.HistoryRefundRepository.GetAllAsync();
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();

            // Lọc các bản ghi hợp lệ
            var depositAppointments = appointments
                .Where(a => a.DepositForApp > 0 && a.CreateAt.HasValue)
                .ToList();

            var validCommissions = commissions
                .Where(c => c.CreateAt.HasValue)
                .ToList();

            var validRefunds = refunds
                .Where(r => r.CreateAt.HasValue)
                .ToList();

            var cancellationPayoutTransactions = transactions
                .Where(t => t.TransactionType == TransactionType.CancellationPayoutArtist && t.CreateAt.HasValue)
                .ToList();

            // Tổng hợp tất cả các năm có dữ liệu
            var years = depositAppointments.Select(a => a.CreateAt.Value.Year)
                .Union(validCommissions.Select(c => c.CreateAt.Value.Year))
                .Union(validRefunds.Select(r => r.CreateAt.Value.Year))
                .Union(cancellationPayoutTransactions.Select(t => t.CreateAt.Value.Year))
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            response.PerYear = new List<RevenuePerYearDto>();
            decimal totalIncomeAllYear = 0;
            decimal totalPayoutForArtistAllYear = 0;

            foreach (var year in years)
            {
                var months = Enumerable.Range(1, 12);

                // Thu nhập từng tháng
                var incomeByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => depositAppointments
                        .Where(a => a.CreateAt.Value.Year == year && a.CreateAt.Value.Month == m)
                        .Sum(a => a.DepositForApp)
                );

                // Lợi nhuận ròng từng tháng
                var netProfitByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => validCommissions
                        .Where(c => c.CreateAt.Value.Year == year && c.CreateAt.Value.Month == m)
                        .Sum(c => c.Amount)
                );

                // Hoàn tiền từng tháng
                var refundByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => validRefunds
                        .Where(r => r.CreateAt.Value.Year == year && r.CreateAt.Value.Month == m)
                        .Sum(r => r.RefundAmount)
                );

                // Payout cho artist khi huỷ từng tháng
                var payoutForArtistByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => cancellationPayoutTransactions
                        .Where(t => t.CreateAt.Value.Year == year && t.CreateAt.Value.Month == m)
                        .Sum(t => t.Amount)
                );

                var totalIncomePerYear = incomeByMonth.Values.Sum();
                var totalNetProfitPerYear = netProfitByMonth.Values.Sum();
                var totalRefundPerYear = refundByMonth.Values.Sum();
                var totalPayoutForArtist = payoutForArtistByMonth.Values.Sum();

                totalIncomeAllYear += totalIncomePerYear;
                totalPayoutForArtistAllYear += totalPayoutForArtist;

                var bestIncome = incomeByMonth.OrderByDescending(x => x.Value).First();
                var bestNetProfit = netProfitByMonth.OrderByDescending(x => x.Value).First();
                var bestRefund = refundByMonth.OrderByDescending(x => x.Value).First();

                response.PerYear.Add(new RevenuePerYearDto
                {
                    Year = year,
                    TotalIncomePerYear = totalIncomePerYear,
                    TotalNetProfitPerYear = totalNetProfitPerYear,
                    TotalRefundPerYear = totalRefundPerYear,
                    TotalPayoutForArtist = totalPayoutForArtist,

                    BestIncome = new MonthValueDto { Month = bestIncome.Key, Value = bestIncome.Value },
                    BestNetProfit = new MonthValueDto { Month = bestNetProfit.Key, Value = bestNetProfit.Value },
                    BestRefund = new MonthValueDto { Month = bestRefund.Key, Value = bestRefund.Value },

                    Income = incomeByMonth,
                    Refund = refundByMonth,
                    NetProfit = netProfitByMonth
                    // Nếu muốn trả về payout từng tháng, thêm: PayoutForArtist = payoutForArtistByMonth
                });
            }

            response.TotalIncomeAllYear = totalIncomeAllYear;
            // Nếu muốn trả về tổng payout cho artist tất cả các năm, thêm property vào response và gán ở đây:
            // response.TotalPayoutForArtistAllYear = totalPayoutForArtistAllYear;

            return response;
        }
    }
}
