using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Constrans;
using Domain.Entities;
using Google.Apis.Auth;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class LoginGoogleHandler : IRequestHandler<LoginGoogleCommand, LoginGoogleResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly GenerateJwtToken _genarateJwtToken;
        private readonly IUnitOfWork _unitOfWork;

        public LoginGoogleHandler(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, GenerateJwtToken genarateJwtToken, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _genarateJwtToken = genarateJwtToken;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginGoogleResponse> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            var response = new LoginGoogleResponse();

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId);
            }
            catch(Exception)
            {
                response.ErrorMessage = "Invalid Google token";
                return response;
            }
            var email = payload.Email;
            var name = payload.Name;

            var user = await _userManager.FindByNameAsync(email);
            if(user == null)
            {
                user = new UserApp
                {
                    Email = email,
                    UserName = email,
                    Name = name,
                    TagName = email.Split('@')[0],
                };
                var resutl = await _userManager.CreateAsync(user);
                if (!resutl.Succeeded)
                {
                    response.ErrorMessage = string.Join(", ", resutl.Errors.Select(e => e.Description));
                    return response;
                }
                await _userManager.AddToRoleAsync(user, UserRole.Customer);

                var userPr = await _unitOfWork.UserAppRepository.FindAsync(x => x.Id == user.Id);
                var singleUser = userPr.FirstOrDefault();
                var userProgress = new Domain.Entities.UserProgress
                {
                    User = singleUser,
                    LastRankUpgradeDate = DateTime.UtcNow
                };

                _unitOfWork.UserProgressRepository.AddAsync(userProgress);
                await _unitOfWork.SaveChangesAsync();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            response.IsSuccess = true;
            response.Email = email;
            response.Name = name;
            var role = await _userManager.GetRolesAsync(user);
            response.JwtToken = _genarateJwtToken.Generate(user, role);

            var refreshToken = _genarateJwtToken.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14); // thời hạn 14 ngày
            await _userManager.UpdateAsync(user);
            response.RefreshToken = refreshToken;

            return response;
        }
    }
}
