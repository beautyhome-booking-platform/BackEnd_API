using Application.UserProgress.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Commands
{
    public class GetUserProgressCommand : IRequest<GetUserProgressResponse>
    {
        public string UserId { get; set; }
    }
}
