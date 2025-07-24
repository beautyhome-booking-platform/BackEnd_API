using Application.UserAccount.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class AcceptArtistCommand : IRequest<AcceptArtistResponse>
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public bool Accept { get; set; } = false;
    }
}
