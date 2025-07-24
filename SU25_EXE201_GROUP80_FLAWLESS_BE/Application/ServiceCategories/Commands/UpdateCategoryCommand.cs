using Application.ServiceCategories.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Commands
{
    public class UpdateCategoryCommand : IRequest<UpdateCategoryResponse>
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
