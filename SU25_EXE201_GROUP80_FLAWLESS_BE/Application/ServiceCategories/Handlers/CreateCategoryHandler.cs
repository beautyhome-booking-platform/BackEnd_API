using Application.ServiceCategories.Commands;
using Application.ServiceCategories.Responses;
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

namespace Application.ServiceCategories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        IUnitOfWork _unitOfWork;
        IClouddinaryStorage _clouddinaryStorage;
        public CreateCategoryHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;
        }
        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCategoryResponse();

            var checkName = ( await _unitOfWork.ServiceCategoriesRepository.FindAsync(sc => sc.Name == request.Name)).FirstOrDefault();
            if (checkName != null)
            {
                response.ErrorMessage = "Name Service Category already exists";
                return response;
            }
            var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);

            var serviceCategory = new Domain.Entities.ServiceCategories
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = imageUrl
            };
            if(serviceCategory != null)
            {
                _unitOfWork.ServiceCategoriesRepository.AddAsync(serviceCategory);
                await _unitOfWork.SaveChangesAsync();

                response.ServiceCategory = new ServiceCategoryDTO
                {
                    Id = serviceCategory.Id,
                    Name = serviceCategory.Name,
                    Description = serviceCategory.Description,
                    ImageUrl = serviceCategory.ImageUrl
                };
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
