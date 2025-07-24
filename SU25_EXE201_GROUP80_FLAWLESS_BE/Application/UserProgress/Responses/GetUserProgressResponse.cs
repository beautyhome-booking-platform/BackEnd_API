using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Responses
{
    public class GetUserProgressResponse : BaseResponse
    {
        public UserProgressDTO UserProgress { get; set; }
    }
}
