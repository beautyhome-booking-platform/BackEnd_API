using Application.ArtistAvailability.Commands;
using Application.ArtistAvailability.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Handlers
{
    public class UpdateAvailabilityHandler : IRequestHandler<UpdateAvailabilityCommand, UpdateAvailabilityResponse>
    {
        IUnitOfWork _unitOfWork;

        public UpdateAvailabilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateAvailabilityResponse> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateAvailabilityResponse();
            bool check = false;

            var exitingAvailability = await _unitOfWork.ArtistAvailabilityRepository.FindAsync(aa => aa.Artist.Id == request.ArtistId);

            foreach (var availability in request.TimeRequests)
            {
                var exactMatch = exitingAvailability
                                .FirstOrDefault(a => a.InvailableDateStart == availability.StartDate && a.InvailableDateEnd == availability.EndDate);
                if (exactMatch != null && exactMatch.Status != availability.Status)
                {
                    exactMatch.Status = availability.Status;
                    _unitOfWork.ArtistAvailabilityRepository.Update(exactMatch);
                    await _unitOfWork.SaveChangesAsync();
                    check = true;
                }
                else
                {
                    var newAvailability = new Domain.Entities.ArtistAvailability
                    {
                        InvailableDateStart = availability.StartDate,
                        InvailableDateEnd = availability.EndDate,
                        Status = availability.Status,
                        Artist = (await _unitOfWork.UserAppRepository.FindAsync(a => a.Id == request.ArtistId)).FirstOrDefault(),
                    };
                    _unitOfWork.ArtistAvailabilityRepository.AddAsync(newAvailability);
                    await _unitOfWork.SaveChangesAsync();
                    check = true;
                }

                if(check)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Update availability failed";
                }
            }
            return response;
        }
    }
}
