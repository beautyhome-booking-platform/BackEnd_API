using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Entities;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class VerifyTwoFactorCodeHandler : IRequestHandler<VerifyTwoFactorCodeCommand, VerifyTwoFactorCodeResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly GenerateJwtToken _generateJwtToken;

        public VerifyTwoFactorCodeHandler(UserManager<UserApp> userManager, GenerateJwtToken generateJwtToken)
        {
            _userManager = userManager;
            _generateJwtToken = generateJwtToken;
        }

        public async Task<VerifyTwoFactorCodeResponse> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
        {
            var response = new VerifyTwoFactorCodeResponse();
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.TwoFactorCode))
            {
                response.ErrorMessage = "Email and two-factor code must be provided.";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.ErrorMessage = "User not found";
                return response;
            }

            // Xác thực mã 2FA (provider: "Email", bạn có thể thay thành "Phone" nếu dùng SMS)
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.TwoFactorCode);
            if (!isValid)
            {
                response.ErrorMessage = "Invalid two-factor authentication code";
                return response;
            }

            // Nếu hợp lệ, tạo JWT token
            var roles = await _userManager.GetRolesAsync(user);
            response.Token = _generateJwtToken.Generate(user, roles);
            response.IsSuccess = true;

            var refreshToken = _generateJwtToken.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _userManager.UpdateAsync(user);
            response.RefreshToken = refreshToken;

            return response;
        }
    }
}
