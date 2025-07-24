using Application.ArtistAvailability.Commands;
using Application.ArtistAvailability.Responses;
using Domain.Constrans;
using Domain.Enum;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Handlers
{
    public class GetAvailabilityHandler : IRequestHandler<GetAvailabilityCommand, GetAvailabilityResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetAvailabilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetAvailabilityResponse> Handle(GetAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var response = new GetAvailabilityResponse();

            DateTime dayStart = request.Date.Date.AddHours(WorkTime.StartHour);
            DateTime dayEnd = request.Date.Date.AddHours(WorkTime.EndHour);

            // Lấy tất cả busy slots (không rảnh) trong ngày
            var allBusySlots = (await _unitOfWork.ArtistAvailabilityRepository.FindAsync(a =>
                                        a.Artist.Id == request.ArtistId &&
                                        a.Status != AvailabilityStatus.Available))
                                        .OrderBy(a => a.InvailableDateStart)
                                        .ToList();

            // Phân loại busy slots
            var bookedSlots = allBusySlots.Where(a => a.Status == AvailabilityStatus.Booked).ToList();
            var unavailableSlots = allBusySlots.Where(a => a.Status == AvailabilityStatus.Unavailable).ToList();

            // Ghép chung để tính khoảng trống
            var combinedBusySlots = bookedSlots.Concat(unavailableSlots)
                .OrderBy(a => a.InvailableDateStart)
                .ToList();

            var freeSlots = new List<(DateTime Start, DateTime End)>();
            DateTime currentStart = dayStart;

            foreach (var busy in combinedBusySlots)
            {
                if (busy.InvailableDateStart > currentStart)
                {
                    freeSlots.Add((currentStart, busy.InvailableDateStart));
                }
                if (busy.InvailableDateEnd > currentStart)
                    currentStart = busy.InvailableDateEnd;
            }

            if (currentStart < dayEnd)
            {
                freeSlots.Add((currentStart, dayEnd));
            }

            var availableSlots = new List<TimeSlot>();

            foreach (var slot in freeSlots)
            {
                DateTime slotStart = slot.Start;

                while (true)
                {
                    // Chỉ kiểm tra khoảng Booked liền kề trước slotStart
                    var prevBookedBusy = bookedSlots.FirstOrDefault(b => b.InvailableDateEnd == slotStart);

                    TimeSpan slotLength = TimeSpan.FromHours((double)request.TotalTime);

                    if (prevBookedBusy != null)
                    {
                        slotLength = slotLength.Add(TimeSpan.FromHours(1));  // Kéo dài thêm 1 giờ
                    }

                    if (slotStart + slotLength <= slot.End)
                    {
                        availableSlots.Add(new TimeSlot
                        {
                            StartDate = slotStart,
                            EndDate = slotStart + slotLength
                        });

                        slotStart = slotStart + slotLength;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            response.TimeSlots = availableSlots;
            response.IsSuccess = true;

            return response;
        }
    }
}
