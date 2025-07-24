using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResponse>
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserHandler(UserManager<UserApp> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUserResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var response = new GetUserResponse();

            List<UserApp> usersToQuery;

            if (request.Role != null)
            {
                // Nếu có Role → lấy theo role
                var roleName = request.Role.ToString();
                usersToQuery = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            }
            else
            {
                // Không có role → lấy toàn bộ
                usersToQuery = _userManager.Users.ToList();
            }

            // Nếu có UserId → lọc theo UserId
            if (!string.IsNullOrEmpty(request.UserId))
            {
                usersToQuery = usersToQuery
                    .Where(u => u.Id == request.UserId)
                    .ToList();
            }

            // Loại bỏ user bị khóa
            usersToQuery = usersToQuery
                .Where(u => u.LockoutEnabled && (u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null))
                .ToList();

            // Phân trang
            var pagedUsers = usersToQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map sang DTO
            response.Users = pagedUsers.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Address = u.Address,
                Gender = u.Gender,
                ImageUrl = u.ImageUrl,
                PhoneNumber = u.PhoneNumber,
                TagName = u.TagName,
            }).ToList();

            response.TotalCount = usersToQuery.Count;
            response.IsSuccess = true;

            return response;
        }
    }
}
