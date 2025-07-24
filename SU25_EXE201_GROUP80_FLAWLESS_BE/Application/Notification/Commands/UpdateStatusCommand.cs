using Application.Notification.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notification.Commands
{
    public class UpdateStatusCommand : IRequest<UpdateStatusResponse>
    {
        public Guid Id { get; set; }
    }
}
