using Application.Post.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class GetPostCommand : IRequest<GetPostResponse>
    {
        public Guid? Id { get; set; }
        public string? SearchContent { get; set; }
        public string? AuthorId { get; set; }
    }
}
