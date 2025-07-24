using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Responses
{
    public class GetServiceReponse : BaseResponse
    {
        public List<ServiceDTO> serviceDTOs { get; set; }
    }
}
