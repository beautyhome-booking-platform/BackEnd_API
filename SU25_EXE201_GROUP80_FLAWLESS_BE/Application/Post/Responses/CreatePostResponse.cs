using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Responses
{
    public class CreatePostResponse : BaseResponse
    {
        public PostDTO Post { get; set; }
    }
}
