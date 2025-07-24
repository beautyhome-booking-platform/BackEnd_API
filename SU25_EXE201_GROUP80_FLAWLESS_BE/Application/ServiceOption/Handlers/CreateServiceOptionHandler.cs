using Application.ServiceOption.Commands;
using Application.ServiceOption.Responses;
using Domain.DTO;
using Domain.Entities;
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
    public class CreateServiceOptionHandler : IRequestHandler<CreateServiceOptionCommand, CreateServiceOptionResponse>
    {
        IUnitOfWork _unitOfWork;
        BlobStorage _blobStorage;
        IClouddinaryStorage _clouddinaryStorage;
        public CreateServiceOptionHandler(IUnitOfWork unitOfWork, BlobStorage blobStorage, IClouddinaryStorage clouddinary)
        {
            _unitOfWork = unitOfWork;
            _blobStorage = blobStorage;
            _clouddinaryStorage = clouddinary;
        }
        public async Task<CreateServiceOptionResponse> Handle(CreateServiceOptionCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateServiceOptionResponse();

            var checkName = (await _unitOfWork.ServiceOptionRepository.FindAsync(sc => sc.Name == request.Name)).FirstOrDefault();
            if(checkName != null)
            {
                response.ErrorMessage = "Name Service Option already exists";
                return response;
            }
            var imageUrl = "";
            if ((request.ImageFile != null))
            {
                imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);
            }
            else
            {
                imageUrl = null;
            }
            var serviceOption = new Domain.Entities.ServiceOption
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = imageUrl,
                Price = request.Price,
                ArtistId = request.ArtistId,
                ServiceId = request.ServiceId,
                Duration = request.Duration
            };
            if(serviceOption != null)
            {
                _unitOfWork.ServiceOptionRepository.AddAsync(serviceOption);
                await _unitOfWork.SaveChangesAsync();
                response.ServiceOptionDTO = new ServiceOptionDTO
                {
                    Id = serviceOption.Id,
                    Name = serviceOption.Name,
                    Description = serviceOption.Description,
                    ImageUrl = serviceOption.ImageUrl,
                    Price = serviceOption.Price,
                    ServiceId = serviceOption.ServiceId,
                    Duration = serviceOption.Duration
                };
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
