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
    public class TotalBookingHandler : IRequestHandler<TotalBookingCommand, TotalBookingResponse>
    {
        IUnitOfWork _unitOfWork;
        public TotalBookingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TotalBookingResponse> Handle(TotalBookingCommand request, CancellationToken cancellationToken)
        {
            var response = new TotalBookingResponse();

            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

            // Chỉ lấy các booking có CreateAt
            var validAppointments = appointments.Where(a => a.CreateAt.HasValue).ToList();

            // Các trạng thái bị huỷ
            var cancelStatuses = new[]
            {
                AppointmentStatus.Canceled,
                AppointmentStatus.Rejected,
                AppointmentStatus.WaitRefund,
                AppointmentStatus.Refunded
            };

            // Lấy tất cả các năm có dữ liệu
            var years = validAppointments
                .Select(a => a.CreateAt.Value.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            response.PerYear = new List<BookingPerYearDto>();
            int totalBookingAllYear = 0;

            foreach (var year in years)
            {
                var months = Enumerable.Range(1, 12);

                // Số booking từng tháng
                var bookingByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => validAppointments.Count(a => a.CreateAt.Value.Year == year && a.CreateAt.Value.Month == m)
                );

                // Số booking bị huỷ từng tháng
                var cancelByMonth = months.ToDictionary(
                    m => new DateTime(year, m, 1).ToString("MMM").ToLower(),
                    m => validAppointments.Count(a =>
                        a.CreateAt.Value.Year == year &&
                        a.CreateAt.Value.Month == m &&
                        cancelStatuses.Contains(a.Status))
                );

                var totalBookingPerYear = bookingByMonth.Values.Sum();
                totalBookingAllYear += totalBookingPerYear;

                var bestBooking = bookingByMonth.OrderByDescending(x => x.Value).First();
                var bestCancel = cancelByMonth.OrderByDescending(x => x.Value).First();

                response.PerYear.Add(new BookingPerYearDto
                {
                    Year = year,
                    TotalBookingPerYear = totalBookingPerYear,
                    BestBooking = new MonthBookingDto { Month = bestBooking.Key, Booking = bestBooking.Value },
                    BestCancel = new MonthCancelDto { Month = bestCancel.Key, Cancel = bestCancel.Value },
                    Booking = bookingByMonth,
                    Cancel = cancelByMonth
                });
            }

            response.TotalBookingAllYear = totalBookingAllYear;
            response.IsSuccess = true;
            return response;
        }
    }
}
