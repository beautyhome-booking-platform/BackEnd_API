using Application.Dashboard.Commands;
using Application.Dashboard.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Handlers
{
    public class GetTotalCustomerHandler : IRequestHandler<GetTotalCustomerCommand, GetTotalCustomerResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetTotalCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetTotalCustomerResponse> Handle(GetTotalCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new GetTotalCustomerResponse();

            var userProgresses = await _unitOfWork.UserProgressRepository.GetAllAsync();
            var users = await _unitOfWork.UserAppRepository.GetAllAsync();
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

            // Join để lấy thông tin user tương ứng với từng customer
            var customerInfos = userProgresses
                .Where(p => p.User.Id != null)
                .Select(p =>
                {
                    var user = users.FirstOrDefault(u => u.Id == p.User.Id);
                    return new
                    {
                        User = user,
                        StartDate = p.CreateAt,
                        LockoutEnabled = user?.LockoutEnabled ?? false,
                        LockoutEnd = user?.LockoutEnd,
                        UserId = p.User.Id
                    };
                })
                .Where(x => x.StartDate.HasValue)
                .ToList();

            // Tổng customer toàn hệ thống
            response.TotalCustomerAllYear = customerInfos.Count();

            // Lấy tất cả các năm có customer bắt đầu hoạt động
            var years = customerInfos
                .Select(x => x.StartDate.Value.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            foreach (var year in years)
            {
                var yearDto = new TotalCustomerPerYearDto { Year = year };

                // Customer đã hoạt động đến hết năm này
                yearDto.TotalPerYear = customerInfos.Count(a => a.StartDate.Value.Year <= year);

                var monthsDict = new Dictionary<string, TotalCustomerMonthDto>();

                for (int m = 1; m <= 12; m++)
                {
                    var monthKey = new DateTime(year, m, 1).ToString("MMM", CultureInfo.InvariantCulture).ToLower();

                    // Customer đã hoạt động đến hết tháng này
                    var totalCustomer = customerInfos.Count(a =>
                        (a.StartDate.Value.Year < year) ||
                        (a.StartDate.Value.Year == year && a.StartDate.Value.Month <= m)
                    );

                    // Customer mới trong tháng này
                    var newCustomer = customerInfos.Count(a =>
                        a.StartDate.Value.Year == year && a.StartDate.Value.Month == m
                    );

                    // Customer bị khóa trong tháng này
                    var now = DateTime.UtcNow;
                    var cancelCustomer = customerInfos.Count(a =>
                        a.LockoutEnabled &&
                        a.LockoutEnd.HasValue &&
                        a.LockoutEnd.Value > now &&
                        a.User != null &&
                        a.User.LockoutEnd.HasValue &&
                        a.User.LockoutEnd.Value.Year == year &&
                        a.User.LockoutEnd.Value.Month == m
                    );

                    // Customer quay lại trong tháng này: có booking trong tháng, và đã từng có booking trước đó
                    var monthAppointments = appointments
                        .Where(ap => ap.CreateAt.HasValue &&
                                     ap.CreateAt.Value.Year == year &&
                                     ap.CreateAt.Value.Month == m)
                        .ToList();

                    var returnCustomer = 0;
                    foreach (var c in customerInfos)
                    {
                        // Có booking trong tháng này
                        var hasBookingThisMonth = monthAppointments.Any(ap => ap.CustomerId == c.UserId);
                        if (!hasBookingThisMonth) continue;

                        // Có booking trước tháng này
                        var hasBookingBefore = appointments.Any(ap =>
                            ap.CustomerId == c.UserId &&
                            ap.CreateAt.HasValue &&
                            (ap.CreateAt.Value.Year < year ||
                             (ap.CreateAt.Value.Year == year && ap.CreateAt.Value.Month < m))
                        );
                        if (hasBookingBefore)
                            returnCustomer++;
                    }

                    monthsDict[monthKey] = new TotalCustomerMonthDto
                    {
                        TotalCustomer = totalCustomer,
                        NewCustomer = newCustomer,
                        ReturnCustomer = returnCustomer,
                        CancelCustomer = cancelCustomer
                    };
                }

                yearDto.Months = monthsDict;
                response.PerYear.Add(yearDto);
            }

            response.IsSuccess = true;
            return response;
        }
    }
}
