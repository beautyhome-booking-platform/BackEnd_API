using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Constrans;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class RegistrationHandler : IRequestHandler<RegistrationCommand, RegistrationResponse>
    {

        UserManager<UserApp> _userManager;
        IUnitOfWork _unitOfWork;

        public RegistrationHandler(UserManager<UserApp> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegistrationResponse> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var response = new RegistrationResponse();
            if (string.IsNullOrEmpty(request.Tagname))
            {
                response.ErrorMessage = "Invalid tag name ";
                return response;
            }
            var checkTagname = await _userManager.Users.FirstOrDefaultAsync(u => u.TagName == request.Tagname, cancellationToken);
            if(checkTagname != null)
            {
                response.ErrorMessage = "Tagname is already";
                return response;
            }
            if(string.IsNullOrEmpty(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
            {
                response.ErrorMessage = "Invalid format email address";
                return response;
            }
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                response.ErrorMessage = "Invalid phone number";
                return response;
            }
            var checkExistsPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
            if(checkExistsPhone != null)
            {
                response.ErrorMessage = "Phone number is already";
                return response;
            }
            if(request.Password != request.ConfirmPassword)
            {
                response.ErrorMessage = "Confirm password incorrect";
                return response;
            }
            var user = new UserApp
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                TagName = request.Tagname,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, UserRole.Customer);
                if (roleResult.Succeeded)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);

                    response.IsSuccess = true;
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
                else
                {
                    response.ErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                }
            }
            else
            {
                response.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return response;
        }
    }
}
