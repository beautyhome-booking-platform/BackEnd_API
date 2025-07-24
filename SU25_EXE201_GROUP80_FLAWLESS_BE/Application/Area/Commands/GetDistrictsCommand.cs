using Application.Area.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Area.Commands
{
    public class GetDistrictsCommand : IRequest<GetDistricsResponse>
    {
        public string City { get; set; }
    }
}
