using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Responses
{
    public class GetServiceResponse : BaseResponse
    {
        public List<ServiceCategoryDTO> ServiceCategory { get; set; }
    }
}
