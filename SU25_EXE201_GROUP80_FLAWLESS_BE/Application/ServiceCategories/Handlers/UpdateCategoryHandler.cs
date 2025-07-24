using Application.ServiceCategories.Commands;
using Application.ServiceCategories.Responses;
using Domain.DTO;
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
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClouddinaryStorage _clouddinaryStorage;

        public UpdateCategoryHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;
        }
        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateCategoryResponse();

            // Tìm category theo Id
            var category = await _unitOfWork.ServiceCategoriesRepository.GetByIdAsync(request.Id);
            if (category == null || category.IsDeleted)
            {
                response.ErrorMessage = "Category not found";
                return response;
            }

            // Kiểm tra trùng tên (nếu thay đổi tên, và tên mới đã tồn tại ở category khác)
            if (!string.IsNullOrEmpty(request.Name) && request.Name != category.Name)
            {
                var existingCategory = (await _unitOfWork.ServiceCategoriesRepository
                    .FindAsync(sc => sc.Name == request.Name && sc.Id != request.Id)).FirstOrDefault();
                if (existingCategory != null)
                {
                    response.ErrorMessage = "Name Service Category already exists";
                    return response;
                }
                category.Name = request.Name;
            }

            // Cập nhật description
            if (request.Description != null)
            {
                category.Description = request.Description;
            }

            // Cập nhật ảnh nếu có
            if (request.ImageFile != null)
            {
                var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);
                category.ImageUrl = imageUrl;
            }

            // Update vào repository
            _unitOfWork.ServiceCategoriesRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
    }
}
