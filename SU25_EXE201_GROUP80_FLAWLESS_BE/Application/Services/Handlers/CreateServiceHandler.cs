using Application.Services.Commands;
using Application.Services.Responses;
using CloudinaryDotNet.Actions;
using Domain.DTO;
using Infrastructure.Storage;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Handlers
{
    public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, CreateServiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClouddinaryStorage _clouddinaryStorage;

        public CreateServiceHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;

        }
        public async Task<CreateServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var reseponse = new CreateServiceResponse();

            var checkName = (await _unitOfWork.ServiceRepository.FindAsync(s => s.Name == request.Name)).FirstOrDefault();
            if(checkName != null)
            {
                reseponse.ErrorMessage = "Name Service already exists";
                return reseponse;
            }

            var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);

            var service = new Domain.Entities.Service
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = imageUrl,
                ServiceCategoryId = request.CategoryId,
            };

            if (service != null)
            {
                _unitOfWork.ServiceRepository.AddAsync(service);
                await _unitOfWork.SaveChangesAsync();
                reseponse.Service = new ServiceDTO
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    ImageUrl = service.ImageUrl,
                    ServiceCategoryId = service.ServiceCategoryId
                };
                reseponse.IsSuccess = true;
            }

            return reseponse;
        }
    }

}
