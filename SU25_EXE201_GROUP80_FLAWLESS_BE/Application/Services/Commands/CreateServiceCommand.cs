using Application.Services.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Commands
{
    public class CreateServiceCommand : IRequest<CreateServiceResponse>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Guid CategoryId { get; set; }
    }
}
