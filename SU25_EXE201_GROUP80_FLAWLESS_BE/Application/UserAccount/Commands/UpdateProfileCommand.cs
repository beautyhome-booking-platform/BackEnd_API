using Application.UserAccount.Responses;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class UpdateProfileCommand : IRequest<UpdateProfileResponse>
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? TagName { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public IFormFile? ImageUrl { get; set; }

    }
}
