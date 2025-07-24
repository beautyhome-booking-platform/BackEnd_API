using Application.Dashboard.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Commands
{
    public class GetTotalCustomerCommand : IRequest<GetTotalCustomerResponse>
    {
    }
}
