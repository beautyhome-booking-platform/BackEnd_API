using Application.UserAccount.Responses;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class RegistrationCommand : IRequest<RegistrationResponse>
    {
        [Required]
        [FromForm]
        public string Name { get; set; }
        [Required]
        [FromForm]
        public string Tagname { get; set; }
        [Required]
        [FromForm]
        public string PhoneNumber { get; set; }

        [Required]
        [FromForm]
        public Gender Gender { get; set; }
        [Required]
        [FromForm]

        public string Email { get; set; }
        [Required]
        [FromForm]
        public string Password { get; set; }
        [Required]
        [FromForm]
        public string ConfirmPassword { get; set; }
    }
}
