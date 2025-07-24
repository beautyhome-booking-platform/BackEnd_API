using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Responses
{
    public class GetServiceOptionReponse : BaseResponse
    {
        public List<ServiceOptionDTO> ServiceOption { get; set; }
    }
}
