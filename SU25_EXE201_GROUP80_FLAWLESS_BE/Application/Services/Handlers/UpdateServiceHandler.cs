using Application.Services.Commands;
using Application.Services.Responses;
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
    public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, UpdateServiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClouddinaryStorage _clouddinaryStorage;

        public UpdateServiceHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;
        }
        public async Task<UpdateServiceResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateServiceResponse();

            // Tìm Service theo Id
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);
            if (service == null || service.IsDeleted)
            {
                response.ErrorMessage = "Service not found";
                response.IsSuccess = false;
                return response;
            }

            // Kiểm tra trùng tên (nếu có Name và khác hiện tại)
            if (!string.IsNullOrEmpty(request.Name) && request.Name != service.Name)
            {
                var existed = (await _unitOfWork.ServiceRepository
                    .FindAsync(s => s.Name == request.Name && s.Id != request.Id)).FirstOrDefault();
                if (existed != null)
                {
                    response.ErrorMessage = "Service name already exists";
                    response.IsSuccess = false;
                    return response;
                }
                service.Name = request.Name;
            }

            // Cập nhật Description
            if (request.Description != null)
                service.Description = request.Description;

            // Cập nhật CategoryId nếu có
            if (request.CategoryId.HasValue)
                service.ServiceCategoryId = request.CategoryId.Value;

            // Cập nhật ảnh nếu có
            if (request.ImageFile != null)
            {
                var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);
                service.ImageUrl = imageUrl;
            }

            _unitOfWork.ServiceRepository.Update(service);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
    }
}
