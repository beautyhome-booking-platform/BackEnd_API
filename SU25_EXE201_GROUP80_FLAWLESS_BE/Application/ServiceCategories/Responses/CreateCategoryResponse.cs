using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Responses
{
    public class CreateCategoryResponse : BaseResponse
    {
        public ServiceCategoryDTO ServiceCategory { get; set; }
    }
}
