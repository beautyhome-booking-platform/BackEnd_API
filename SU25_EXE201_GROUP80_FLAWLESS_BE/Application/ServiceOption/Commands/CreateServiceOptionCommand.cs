using Application.ServiceOption.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Commands
{
    public class CreateServiceOptionCommand : IRequest<CreateServiceOptionResponse>
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Duration { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string ArtistId { get; set; }
        [Required]
        public Guid ServiceId { get; set; }
    }
}
