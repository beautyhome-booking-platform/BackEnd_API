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
    public class UpdateServiceOptionHandler : IRequestHandler<UpdateServiceOptionCommand, UpdateServiceOptionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClouddinaryStorage _clouddinaryStorage;

        public UpdateServiceOptionHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;
        }

        public async Task<UpdateServiceOptionResponse> Handle(UpdateServiceOptionCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateServiceOptionResponse();

            // Tìm ServiceOption theo Id
            var serviceOption = await _unitOfWork.ServiceOptionRepository.GetByIdAsync(request.Id);
            if (serviceOption == null || serviceOption.IsDeleted)
            {
                response.ErrorMessage = "Service option not found";
                response.IsSuccess = false;
                return response;
            }

            // Kiểm tra trùng tên (nếu thay đổi tên, và tên mới đã tồn tại ở option khác)
            if (!string.IsNullOrEmpty(request.Name) && request.Name != serviceOption.Name)
            {
                var existed = (await _unitOfWork.ServiceOptionRepository
                    .FindAsync(so => so.Name == request.Name && so.Id != request.Id)).FirstOrDefault();
                if (existed != null)
                {
                    response.ErrorMessage = "Service option name already exists";
                    response.IsSuccess = false;
                    return response;
                }
                serviceOption.Name = request.Name;
            }

            // Cập nhật description
            if (request.Description != null)
                serviceOption.Description = request.Description;

            // Cập nhật giá
            if(request.Price != null)
            serviceOption.Price = (decimal)request.Price;

            // Cập nhật artist và serviceId (nếu cho phép đổi)
            if(request.ServiceId != null)
            serviceOption.ServiceId = (Guid)request.ServiceId;

            // Cập nhật ảnh nếu có
            if (request.ImageFile != null)
            {
                var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);
                serviceOption.ImageUrl = imageUrl;
            }
            if (request.Duration != null)
                serviceOption.Duration = (int)request.Duration;

            // Update vào repository
            _unitOfWork.ServiceOptionRepository.Update(serviceOption);
            await _unitOfWork.SaveChangesAsync();
         
            response.IsSuccess = true;

            return response;
        }
    }
}
