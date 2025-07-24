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
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly GenerateJwtToken _generateJwtToken;
        public RefreshTokenHandler(UserManager<UserApp> userManager, GenerateJwtToken generateJwtToken)
        {
            _userManager = userManager;
            _generateJwtToken = generateJwtToken;
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new RefreshTokenResponse();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.ErrorMessage = "User not found";
                return response;
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                response.ErrorMessage = "Invalid or expired refresh token";
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _generateJwtToken.Generate(user, roles);

            // Tạo refresh token mới, lưu lại
            var newRefreshToken = _generateJwtToken.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _userManager.UpdateAsync(user);

            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;
            response.IsSuccess = true;

            return response;
        }
    }
}

