using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.DTO;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class GetInformationArtistHandler : IRequestHandler<GetInformationArtistCommand, GetInformationArtistResponse>
    {
        IUnitOfWork _unitOfWork;
        UserManager<UserApp> _userManager;
        public GetInformationArtistHandler(IUnitOfWork unitOfWork, UserManager<UserApp> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<GetInformationArtistResponse> Handle(GetInformationArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new GetInformationArtistResponse();

            // Load all related entities in advance
            var artistEntities = await _unitOfWork.InformationArtistRepository
                .FindAsync(null, query => query
                    .Include(a => a.ArtistProgress).ThenInclude(p => p.Artist)
                    .Include(a => a.Area));
            if (request.RequestStatus.HasValue)
            {
                artistEntities = artistEntities
                    .Where(a => a.ArtistProgress != null
                             && a.ArtistProgress.Note == request.RequestStatus.Value)
                    .ToList();
            }
            var bankInfos = await _unitOfWork.BankInfoRepository.GetAllAsync();
            var serviceOptions = await _unitOfWork.ServiceOptionRepository.GetAllAsync();
            var certificates = await _unitOfWork.CertificateRepository.GetAllAsync();
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();
            var appointments = await _unitOfWork.AppointmentRepository
                .FindAsync(null, query => query
                    .Include(a => a.Customer)   
                    .Include(a => a.AppointmentsDetails)
                        .ThenInclude(ad => ad.ServiceOption));
            var availabilities = await _unitOfWork.ArtistAvailabilityRepository.GetAllAsync();

            var data = new List<ArtistInfoDTO>();

            foreach (var artist in artistEntities)
            {
                var user = artist.ArtistProgress?.Artist;
                if (user == null) continue;

                var artistId = user.Id;

                // Get roles (if needed)
                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.FirstOrDefault() ?? "";

                // Get appointments for this artist
                var artistAppointments = appointments
                    .Where(a => a.ArtistMakeupId == artistId)
                    .ToList();

                var scheduleList = new List<Scheduledto>();

                // 1. Add all real appointment schedules (from Appointment)
                foreach (var appointment in artistAppointments)
                {
                    foreach (var ad in appointment.AppointmentsDetails)
                    {
                        // Find availability with Note = AppointmentId (to get start/end/duration)
                        var availability = availabilities.FirstOrDefault(av =>
                            av.Artist.Id == artistId &&
                            av.Note == appointment.Id.ToString());

                        string time = availability != null
                            ? availability.InvailableDateStart.ToString("HH:mm")
                            : appointment.AppointmentDate.ToString("HH:mm");

                        string duration = "N/A";
                        if (availability != null)
                        {
                            TimeSpan ts = availability.InvailableDateEnd - availability.InvailableDateStart;
                            duration = $"{(int)ts.TotalHours}h {ts.Minutes}m";
                        }

                        scheduleList.Add(new Scheduledto
                        {
                            AppointmentId = appointment.Id.ToString(),
                            Customer = new CustomerDto
                            {
                                Id = appointment.Customer?.Id,
                                Name = appointment.Customer?.Name,
                                Avatar = appointment.Customer?.ImageUrl,
                                Phone = appointment.Customer?.PhoneNumber,
                                Note = appointment.Note,
                                Address = appointment.Address
                            },
                            Service = ad.ServiceOption?.Name ?? "",
                            Date = availability != null
                                ? availability.InvailableDateStart.ToString("yyyy-MM-dd")
                                : appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                            Time = time,
                            Duration = duration,
                            Status = availability.Status // Trạng thái của cuộc hẹn (Pending, Completed, Cancel...)
                        });
                    }
                }

                var appointmentIds = artistAppointments.Select(a => a.Id.ToString()).ToHashSet();
                // 2. Add all ArtistAvailability: Booking và Unavailable
                var bookingOrUnavailableSlots = availabilities
                    .Where(av => av.Artist.Id == artistId
                        && (av.Status == AvailabilityStatus.Booked || av.Status == AvailabilityStatus.Unavailable)
                        && (string.IsNullOrEmpty(av.Note) || !appointmentIds.Contains(av.Note)))
                    .ToList();

                foreach (var av in bookingOrUnavailableSlots)
                {
                    // Kiểm tra có liên kết appointment không
                    var relatedAppointment = artistAppointments
                        .FirstOrDefault(app => app.Id.ToString() == av.Note);

                    CustomerDto customer = null;
                    string service = "";
                    if (relatedAppointment != null)
                    {
                        var firstDetail = relatedAppointment.AppointmentsDetails.FirstOrDefault();
                        customer = new CustomerDto
                        {
                            Id = relatedAppointment.Customer?.Id,
                            Name = relatedAppointment.Customer?.Name,
                            Avatar = relatedAppointment.Customer?.ImageUrl,
                            Phone = relatedAppointment.Customer?.PhoneNumber,
                            Note = relatedAppointment.Note,
                            Address = relatedAppointment.Address
                        };
                        service = firstDetail?.ServiceOption?.Name ?? "";
                    }

                    scheduleList.Add(new Scheduledto
                    {
                        AppointmentId = av.Note ?? "", // có thể là null nếu không liên kết appointment
                        Customer = customer,           // null nếu là unavailable không phải booking
                        Service = service,             // "" nếu unavailable
                        Date = av.InvailableDateStart.ToString("yyyy-MM-dd"),
                        Time = av.InvailableDateStart.ToString("HH:mm"),
                        Duration = $"{(int)(av.InvailableDateEnd - av.InvailableDateStart).TotalHours}h {(av.InvailableDateEnd - av.InvailableDateStart).Minutes}m",
                        Status = av.Status        // Status lấy trực tiếp từ availability (Booking/Unavailable)
                    });
                }
                var check = await _unitOfWork.ArtistAvailabilityRepository.GetAllAsync();
                // All feedback for artist's services
                var artistServiceOptions = serviceOptions.Where(s => s.ArtistId == artistId).ToList();
                var artistFeedbacks = feedbacks
                    .Where(fb => artistServiceOptions.Select(s => s.Id).Contains(fb.ServiceOptionId))
                    .ToList();
                double averageRating = artistFeedbacks.Any()
                        ? artistFeedbacks.Average(fb => fb.Rating)
                        : 0;

                var dto = new ArtistInfoDTO
                {
                    IdArtist = artistId,
                    NameArtist = user.Name,
                    Avatar = user.ImageUrl,
                    Specialty = "Make up",
                    Status = 0,
                    Gender = user.Gender,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    Dob = artist.CreateAt.ToString(),
                    Address = user.Address,
                    AreaBook = $"{artist.Area?.City}, {artist.Area?.District}",
                    Note = artist.ArtistProgress?.Note.ToString(),
                    AboutArtist = artist.CommonCosmetics,
                    TimeJoin = artist.CreateAt.ToString(),
                    ReviewCount = artist.ArtistProgress?.TotalCompletedAppointments,
                    Rating = averageRating,
                    Experience = $"{artist.YearsOfExperience} years",
                    ScheduleList = scheduleList,
                    TotalIncome = artist.ArtistProgress?.TotalReceive,
                    TotalBooked = artist.ArtistProgress?.TotalCompletedAppointments,
                    TotalCancel = artist.ArtistProgress?.TotalCancellations,
                    BankAccount = bankInfos
                        .Where(b => b.User.Id == artistId)
                        .Select(b => new BankAccountDTO
                        {
                            Bank = b.BankName,
                            Stk = b.AccountNumber,
                            Name = b.AccountHolder
                        }).FirstOrDefault(),
                    Services = serviceOptions
                        .Where(s => s.ArtistId == artistId)
                        .Select(s => new ServiceOptionDTO
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Price = s.Price,
                            Description = s.Description,
                            ImageUrl = s.ImageUrl,
                            ServiceId = s.ServiceId,
                            DiscountPercent = s.DiscountPercent
                        }).ToList(),
                    Certificate = certificates
                        .Where(c => c.InformationArtistId == artist.Id)
                        .Select(c => new CertificateDTO
                        {
                            Name = c.DegreeName,
                            ImageUrl = c.ImageUrl,
                            Institution = c.Institution,
                            Description = c.Description
                        }).ToList(),
                };

                data.Add(dto);
            }

            response.Artists = data;
            response.IsSuccess = true;

            return response;
        }
    }
}
