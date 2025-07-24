using Application.ArtistAvailability.Commands;
using Application.ArtistAvailability.Responses;
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
    public class GetScheduleHandler : IRequestHandler<GetScheduleCommand, GetScheduleResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetScheduleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetScheduleResponse> Handle(GetScheduleCommand request, CancellationToken cancellationToken)
        {
            var response = new GetScheduleResponse();

            var schedules = await _unitOfWork.ArtistAvailabilityRepository.FindAsync(aa => 
                                                                                     aa.Artist.Id == request.ArtistId 
                                                                                     && (aa.Status == AvailabilityStatus.Unavailable ||
                                                                                         aa.Status == AvailabilityStatus.Booked));
            response.Schedule = schedules.Select(aa => new Time
            {
                StartDate = aa.InvailableDateStart,
                EndDate = aa.InvailableDateEnd,
                Status = aa.Status
            }).ToList();
            response.IsSuccess = true;

            return response;
        }
    }
}
