using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Area.Responses
{
    public class GetDistricsResponse : BaseResponse
    {
        public List<AreaDTO> Districts { get; set; } 
    }
}
