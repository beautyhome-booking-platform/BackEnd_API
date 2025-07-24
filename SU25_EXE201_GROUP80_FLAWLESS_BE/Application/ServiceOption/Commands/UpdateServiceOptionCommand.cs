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
    public class UpdateServiceOptionCommand : IRequest<UpdateServiceOptionResponse>
    {
        [Required]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public decimal? Price { get; set; }
        public Guid? ServiceId { get; set; }
    }
}
