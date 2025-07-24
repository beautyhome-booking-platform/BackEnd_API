using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class GetArtistProgressHandler : IRequestHandler<GetArtistProgressCommand, GetArtistProgressResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetArtistProgressHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetArtistProgressResponse> Handle(GetArtistProgressCommand request, CancellationToken cancellationToken)
        {
            var response = new GetArtistProgressResponse();

            if (!Guid.TryParse(request.ArtistId, out var artistIdGuid))
            {
                throw new ArgumentException("Invalid ArtistId format");
            }
            var artistProgress = (await _unitOfWork.ArtistProgressRepository
                .FindAsync(ap => (ap.Artist.Id == request.ArtistId) && ap.IsActive))
                .FirstOrDefault();

            if ((artistProgress == null))
            {
                response.ErrorMessage = "User not found";
                return response;
            }

            var artistProgressDTO = new ArtitstProgressDTO
            {
                Id = artistProgress.Id,
                Points = artistProgress.Points,
                TotalCompletedAppointments = artistProgress.TotalCompletedAppointments,
                TotalCancellations = artistProgress.TotalCancellations,
                LastRankUpgradeDate = artistProgress.LastRankUpgradeDate,
                Rank = artistProgress.Rank,
                IsActive = artistProgress.IsActive,
                Note = artistProgress.Note,
                TotalReceive = artistProgress.TotalReceive
            };
            response.ArtistProgress = artistProgressDTO;
            response.IsSuccess = true;
            return response;
        }
    }
}
