using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Entities;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IClouddinaryStorage _cloudinary;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProfileHandler(UserManager<UserApp> userManager, IClouddinaryStorage cloudinary, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _cloudinary = cloudinary;
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateProfileResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateProfileResponse();

            var user = (await _unitOfWork.UserAppRepository.FindAsync(u => u.Id == request.Id)).FirstOrDefault();
            //var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found";
                return response;
            }

            // 2. Update các trường (có thể kiểm tra trùng TagName, Phone, Email nếu muốn)
            if(!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;
            if (!string.IsNullOrEmpty(request.TagName)) {
                var checkTag = await _unitOfWork.UserAppRepository.FindAsync(x => x.TagName == request.TagName);
                if (checkTag.Any())
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "TagName already exists";
                    return response;
                }
                user.TagName = request.TagName;
            }
            
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var checkPhone = await _unitOfWork.UserAppRepository.FindAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (checkPhone.Any())
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Phone number already exists";
                    return response;
                }
                user.PhoneNumber = request.PhoneNumber;
            }
                
            if (request.Gender != null)
                user.Gender = (Domain.Enum.Gender)request.Gender;
            if (!string.IsNullOrEmpty(request.Address))
                user.Address = request.Address;
            if (request.ImageUrl != null)
            {
                var url = await _cloudinary.UploadImageAsync(request.ImageUrl);
                user.ImageUrl = url;
            }

            
            _unitOfWork.UserAppRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;

            return response;
        }
    }
}
