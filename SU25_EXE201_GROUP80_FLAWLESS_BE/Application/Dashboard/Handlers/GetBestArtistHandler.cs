using Application.Dashboard.Commands;
using Application.Dashboard.Responses;
using Domain.Enum;
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
    public class GetBestArtistHandler : IRequestHandler<GetBestArtistCommand, GetBestArtistResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetBestArtistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetBestArtistResponse> Handle(GetBestArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new GetBestArtistResponse();

            // Load all necessary data
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            var users = await _unitOfWork.UserAppRepository.GetAllAsync();
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();

            // Only consider appointments with CreateAt and ArtistMakeupId
            var validAppointments = appointments
                .Where(a => a.CreateAt.HasValue && !string.IsNullOrEmpty(a.ArtistMakeupId))
                .ToList();

            // Define cancel statuses
            var cancelStatuses = new[]
            {
            AppointmentStatus.Canceled,
            AppointmentStatus.Rejected,
            AppointmentStatus.WaitRefund,
            AppointmentStatus.Refunded
        };

            // Get all years
            var years = validAppointments
                .Select(a => a.CreateAt.Value.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            

            foreach (var year in years)
            {
                var yearDto = new BestArtistYearDto { Year = year };
                var monthsDict = new Dictionary<string, List<BestArtistMonthDto>>();

                for (int m = 1; m <= 12; m++)
                {
                    var monthKey = new DateTime(year, m, 1).ToString("MMM", CultureInfo.InvariantCulture).ToLower();

                    // Filter appointments for this month/year
                    var monthAppointments = validAppointments
                        .Where(a => a.CreateAt.Value.Year == year && a.CreateAt.Value.Month == m)
                        .ToList();

                    // Group by artist
                    var artistGroups = monthAppointments
                        .GroupBy(a => a.ArtistMakeupId)
                        .Select(g =>
                        {
                            var artistId = g.Key;
                            var artist = users.FirstOrDefault(u => u.Id == artistId);

                            // Booking count
                            var totalBooking = g.Count();

                            // Cancel count
                            var totalCancel = g.Count(a => cancelStatuses.Contains(a.Status));

                            // Revenue: sum AmountToPayForArtist (or use your business logic)
                            var totalRevenue = g.Sum(a => a.AmountToPayForArtist);

                            // Avg rating: from feedbacks for this artist in this month
                            var artistAppointmentIds = g.Select(a => a.Id).ToList();
                            var artistFeedbacks = feedbacks
                                .Where(f => artistAppointmentIds.Contains(f.AppoinmentId) && f.CreateAt.HasValue && f.CreateAt.Value.Year == year && f.CreateAt.Value.Month == m)
                                .ToList();

                            double avgRating = artistFeedbacks.Any() ? artistFeedbacks.Average(f => f.Rating) : 0;

                            return new BestArtistMonthDto
                            {
                                IdAr = artist?.Id ?? "",
                                NameAr = artist?.Name ?? "",
                                AvatarAr = artist?.ImageUrl ?? "",
                                Status = artist != null ? (artist.LockoutEnabled ? 0 : 1) : 0, // Example: 1-active, 0-locked
                                AvgRating = avgRating,
                                TotalRevenueInMonth = totalRevenue,
                                TotalBookingInMonth = totalBooking,
                                TotalCancelInMonth = totalCancel
                            };
                        })
                        .OrderByDescending(x => x.TotalBookingInMonth)
                        .Take(5)
                        .ToList();

                    monthsDict[monthKey] = artistGroups;
                }

                yearDto.Months = monthsDict;
                response.BestArtist.Add(yearDto);
                response.IsSuccess = true;
            }

            return response;
        }
    }
}
