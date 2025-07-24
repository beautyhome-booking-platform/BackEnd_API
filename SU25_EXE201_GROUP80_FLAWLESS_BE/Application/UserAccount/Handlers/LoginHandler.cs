using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        UserManager<UserApp> _userManager;
        SignInManager<UserApp> _signInManager;
        GenerateJwtToken _generateJwtToken;
        IEmailSender _emailSender;

        public LoginHandler(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, GenerateJwtToken generateJwtToken, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateJwtToken = generateJwtToken;
            _emailSender = emailSender;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new LoginResponse();

            if (string.IsNullOrEmpty(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
            {
                response.ErrorMessage = "Invalid email address";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || (!user.LockoutEnabled && user.LockoutEnd >= DateTime.Now))
            {
                response.ErrorMessage = "User or password is not valid";
                return response;
            }

            // Kiểm tra mật khẩu trước
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                response.ErrorMessage = "User or password is not correct";
                return response;
            }

            // Kiểm tra bật 2FA chưa
            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var twoFactorProvider = "Email";  // hoặc "Phone" nếu dùng SMS

                // Tạo token 2FA
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, twoFactorProvider);

                // Gửi mã 2FA qua email (giả sử bạn có IEmailSender được inject)
                await _emailSender.SendEmailAsync(user.Email, "Your 2FA code", $"Your verification code is: {token}");

                // Trả về response yêu cầu nhập mã 2FA
                response.IsSuccess = true;
                response.RequiresTwoFactor = true;
                response.Message = "Two-factor authentication required. Please enter the code sent to your email.";

                return response;
            }
            else
            {
                // Không bật 2FA, cấp token ngay
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                var refreshToken = _generateJwtToken.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14); // thời hạn 14 ngày
                await _userManager.UpdateAsync(user);

                response.Name = user.Name;
                response.Email = user.Email;
                response.Address = user.Address;
                response.PhoneNumber = user.PhoneNumber;
                response.Role = role;
                response.IsSuccess = true;
                response.Token = _generateJwtToken.Generate(user, roles);
                response.RefreshToken = refreshToken;

                return response;
            }
        }
    }
}
