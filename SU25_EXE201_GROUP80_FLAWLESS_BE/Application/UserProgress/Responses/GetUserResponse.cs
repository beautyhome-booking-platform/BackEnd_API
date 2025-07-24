using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Responses
{
    public class GetUserResponse : BaseResponse
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
        public int TotalCount { get; set; }
    }
}
