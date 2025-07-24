using Application.Post.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class UpdatePostCommand : IRequest<UpdatePostResponse>
    {
        [Required]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Tags { get; set; }
        public string? Content { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Guid? ServiceOptionId { get; set; }
    }
}
