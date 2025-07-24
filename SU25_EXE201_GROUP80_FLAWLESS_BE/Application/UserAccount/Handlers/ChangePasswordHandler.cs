using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        public ChangePasswordHandler(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new ChangePasswordResponse();

            // 1. Tìm user
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found.";
                return response;
            }

            // 2. Kiểm tra xác nhận password mới
            if (request.NewPassword != request.ConfirmPassword)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "New password and confirmation do not match.";
                return response;
            }

            // 3. Đổi password
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                response.ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return response;
            }

            response.IsSuccess = true;
            return response;
        }
    }
}
