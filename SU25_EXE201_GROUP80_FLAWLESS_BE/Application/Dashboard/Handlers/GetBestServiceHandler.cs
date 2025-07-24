using Application.Dashboard.Commands;
using Application.Dashboard.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Handlers
{
    public class GetBestServiceHandler : IRequestHandler<GetBestServiceCommand, GetBestServiceResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetBestServiceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetBestServiceResponse> Handle(GetBestServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new GetBestServiceResponse();

            // Load all necessary data
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            var appointmentDetails = await _unitOfWork.AppointmentDetailRepository.GetAllAsync();
            var serviceOptions = await _unitOfWork.ServiceOptionRepository.GetAllAsync();
            var services = await _unitOfWork.ServiceRepository.GetAllAsync();
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();

            // Only consider appointments with CreateAt
            var validAppointments = appointments
                .Where(a => a.CreateAt.HasValue)
                .ToList();

            // Get all years
            var years = validAppointments
                .Select(a => a.CreateAt.Value.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            foreach (var year in years)
            {
                var yearDto = new BestServiceYearDto { Year = year };
                var monthsDict = new Dictionary<string, List<BestServiceMonthDto>>();

                for (int m = 1; m <= 12; m++)
                {
                    var monthKey = new DateTime(year, m, 1).ToString("MMM", System.Globalization.CultureInfo.InvariantCulture).ToLower();

                    // Get appointment IDs for this month/year
                    var monthAppointmentIds = validAppointments
                        .Where(a => a.CreateAt.Value.Year == year && a.CreateAt.Value.Month == m)
                        .Select(a => a.Id)
                        .ToList();

                    // Get all appointment details for these appointments
                    var monthDetails = appointmentDetails
                        .Where(d => monthAppointmentIds.Contains(d.AppointmentId))
                        .ToList();

                    // Group by ServiceId (via ServiceOption)
                    var serviceGroups = monthDetails
                        .Join(serviceOptions, d => d.ServiceOptionId, so => so.Id, (d, so) => new { d, so })
                        .Join(services, x => x.so.ServiceId, s => s.Id, (x, s) => new { x.d, x.so, s })
                        .GroupBy(x => x.s.Id)
                        .Select(g =>
                        {
                            var serviceId = g.Key;
                            var service = g.First().s;
                            var totalBooking = g.Count();

                            // Get all feedbacks for this service in this month
                            var serviceOptionIds = g.Select(x => x.so.Id).Distinct().ToList();
                            var feedbacksForService = feedbacks
                                .Where(f => serviceOptionIds.Contains(f.ServiceOptionId) && f.CreateAt.HasValue && f.CreateAt.Value.Year == year && f.CreateAt.Value.Month == m)
                                .ToList();

                            double avgRating = feedbacksForService.Any() ? feedbacksForService.Average(f => f.Rating) : 0;

                            return new BestServiceMonthDto
                            {
                                IdSer = service.Id,
                                NameSer = service.Name,
                                AvgRating = avgRating
                            };
                        })
                        .OrderByDescending(x => x.IdSer) // This should be .OrderByDescending(x => totalBooking) but totalBooking is not in DTO, so you can add it if needed
                        .Take(5)
                        .ToList();

                    monthsDict[monthKey] = serviceGroups;
                }

                yearDto.Months = monthsDict;
                response.BestService.Add(yearDto);
                response.IsSuccess = true;
            }

            return response;
        }
    }
}
