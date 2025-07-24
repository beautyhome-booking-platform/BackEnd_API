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
    public class GetAdvanceInformationArtistHandler : IRequestHandler<GetAdvanceInformationArtistCommand, GetAdvanceInformationArtistResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAdvanceInformationArtistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetAdvanceInformationArtistResponse> Handle(GetAdvanceInformationArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new GetAdvanceInformationArtistResponse();

            if (!Guid.TryParse(request.ArtistId, out var artistIdGuid))
            {
                throw new ArgumentException("Invalid ArtistId format");
            }
            var artistProgress = (await _unitOfWork.ArtistProgressRepository
                        .FindAsync(ap => (ap.Artist.Id == request.ArtistId) && ap.IsActive))
                        .FirstOrDefault();

            if ((artistProgress == null))
            {
                response.ErrorMessage = "User is not a artist or profile have been locked";
                return response;
            }
            var artistAdvanInformation = (await _unitOfWork.InformationArtistRepository.FindAsync(ia => ia.ArtistProgressId == artistProgress.Id)).FirstOrDefault();
            var area = await _unitOfWork.AreaRepository.GetByIdAsync((Guid)artistAdvanInformation.AreaId);
            var artistProgressDTO = new ArtistAdvanceInformationDTO
            {
                Id = artistAdvanInformation.Id,
                AreaDistrict = area.District,
                AreaCity = area.City,
                AcceptUrgentBooking = artistAdvanInformation.AcceptUrgentBooking,
                AreaId = artistAdvanInformation.AreaId,
                CCCD = (List<string>)artistAdvanInformation.CCCD,
                CommonCosmetics = artistAdvanInformation.CommonCosmetics,
                MaxPrice = artistAdvanInformation.MaxPrice,
                MinPrice = artistAdvanInformation.MinPrice,
                PortfolioImages = (List<string>)artistAdvanInformation.PortfolioImages,
                TermsAccepted = artistAdvanInformation.TermsAccepted,
                TermsAcceptedDate = artistAdvanInformation.TermsAcceptedDate,
                YearsOfExperience = artistAdvanInformation.YearsOfExperience,
            };

            response.IsSuccess = true;
            response.ArtistAdvanDTO = artistProgressDTO;
            return response;
        }
    }
}
