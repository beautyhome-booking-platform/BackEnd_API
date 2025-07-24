using Application.ServiceCategories.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Commands
{
    public class GetCategoryCommand : IRequest<GetServiceResponse>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}
