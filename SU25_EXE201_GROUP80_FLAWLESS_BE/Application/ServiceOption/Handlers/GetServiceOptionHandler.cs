using Application.ServiceOption.Commands;
using Application.ServiceOption.Responses;
using Domain.DTO;
using Infrastructure.Storage;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Handlers
{
    public class GetServiceOptionHandler : IRequestHandler<GetServiceOptionCommand, GetServiceOptionReponse>
    {
        IUnitOfWork _unitOfWork;
        BlobStorage _blobStorage;
        public GetServiceOptionHandler(IUnitOfWork unitOfWork, BlobStorage blobStorage)
        {
            _unitOfWork = unitOfWork;
            _blobStorage = blobStorage;
        }
        public async Task<GetServiceOptionReponse> Handle(GetServiceOptionCommand request, CancellationToken cancellationToken)
        {
            var response = new GetServiceOptionReponse();

            var serviceOption = await _unitOfWork.ServiceOptionRepository.FindAsync(so => 
                                                                                    (request.Id == null || so.Id == request.Id) && 
                                                                                    (string.IsNullOrEmpty(request.Name) || so.Name.Contains(request.Name)) &&
                                                                                    (so.ServiceId == request.ServiceId || request.ServiceId == null) &&
                                                                                    (
                                                                                    (request.MinPrice != null && request.MaxPrice != null && so.Price >= request.MinPrice && so.Price <= request.MaxPrice)
                                                                                    || (request.MinPrice == null || request.MaxPrice == null)
                                                                                    )&&
                                                                                    (!so.IsDeleted)
                                                                                    &&(string.IsNullOrEmpty(request.ArtistId) || so.ArtistId == request.ArtistId)
                                                                                    );
            if (serviceOption.Count() > 0)
            {
                var serviceOptionDTO = serviceOption.Select(so => new ServiceOptionDTO
                {
                    Id = so.Id,
                    Name = so.Name,
                    Description = so.Description,
                    Price = so.Price,
                    ImageUrl = so.ImageUrl,
                    ServiceId = so.ServiceId,
                    Duration = so.Duration,
                    ArtistId = so.ArtistId
                }).ToList();
                response.IsSuccess = true;
                response.ServiceOption = serviceOptionDTO;
            }
            else
            {
                response.ErrorMessage = "Service Option not found or not have any service options";
                return response;
            }
            return response;
        }
    }

}
