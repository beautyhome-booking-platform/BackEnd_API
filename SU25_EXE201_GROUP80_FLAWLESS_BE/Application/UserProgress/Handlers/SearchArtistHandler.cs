using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.DTO;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class SearchArtistHandler : IRequestHandler<SearchArtistCommand, SearchArtistResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchArtistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SearchArtistResponse> Handle(SearchArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new SearchArtistResponse();


            bool? isActive = null;
            if (request.RequestStatus == RequestStatus.Accepted)
            {
                isActive = true;
            }
            else if (request.RequestStatus == RequestStatus.Requested || request.RequestStatus == RequestStatus.Rejected)
            {
                isActive = false;
            }

            if (string.IsNullOrEmpty(request.SearchContent))
            {
                
                var pagedArtists = await _unitOfWork.ArtistProgressRepository.FindAsync(ap =>
                    (isActive == null || ap.IsActive == isActive) &&
                    !ap.IsDeleted &&
                    (request.RequestStatus == null || ap.Note == request.RequestStatus));

                var sortedArtists = pagedArtists
                    .OrderByDescending(ap => ap.TotalCompletedAppointments)
                    .Select(ap => new ArtistDTO
                    {
                        Id = ap.Artist.Id,
                        Name = ap.Artist.Name,
                        TagName = ap.Artist.TagName,
                        Gender = ap.Artist.Gender,
                        Email = ap.Artist.Email,
                        Address = ap.Artist.Address,
                        PhoneNumber = ap.Artist.PhoneNumber
                    }).ToList();

                response.Artists = sortedArtists;
                response.TotalCount = pagedArtists.Count();
                response.IsSuccess = true;
            }
            else
            {
                
                var userIds = _unitOfWork.UserAppRepository.Find(
                        u =>
                            (u.Name.Contains(request.SearchContent) || u.TagName.Contains(request.SearchContent)) &&
                            u.LockoutEnabled &&
                            (u.LockoutEnd == null || u.LockoutEnd < DateTime.Now),
                        request.PageNumber,
                        request.PageSize)
                    .Select(u => u.Id)
                    .ToList();

                var pagedArtistProgresses = _unitOfWork.ArtistProgressRepository
                    .Find(ap =>
                        userIds.Contains(ap.Artist.Id) &&
                        (isActive == null || ap.IsActive == isActive) &&
                        !ap.IsDeleted &&
                        (request.RequestStatus == null || ap.Note == request.RequestStatus),
                        request.PageNumber,
                        request.PageSize);

                var artistDTOs = pagedArtistProgresses
                    .Select(ap => new ArtistDTO
                    {
                        Id = ap.Artist.Id,
                        Name = ap.Artist.Name,
                        TagName = ap.Artist.TagName,
                        Gender = ap.Artist.Gender,
                        Email = ap.Artist.Email,
                        Address = ap.Artist.Address,
                        PhoneNumber = ap.Artist.PhoneNumber
                    }).ToList();

                response.Artists = artistDTOs;
                response.TotalCount = pagedArtistProgresses.TotalItemCount;
                response.IsSuccess = true;
            }

            return response;
        }
    }
}
