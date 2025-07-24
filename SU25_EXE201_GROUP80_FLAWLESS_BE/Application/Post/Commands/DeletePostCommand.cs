using Application.Post.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands
{
    public class DeletePostCommand : IRequest<DeletePostResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
