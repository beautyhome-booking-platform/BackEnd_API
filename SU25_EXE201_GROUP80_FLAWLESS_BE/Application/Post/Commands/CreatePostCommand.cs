using Application.Post.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class CreatePostCommand : IRequest<CreatePostResponse>
    {
        public string Title { get; set; }
        public string? Tags { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
        public string AuthorId { get; set; } 
        public Guid? ServiceOptionId { get; set; }
    }
}
