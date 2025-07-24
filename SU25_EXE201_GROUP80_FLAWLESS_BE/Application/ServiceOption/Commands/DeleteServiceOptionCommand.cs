using Application.ServiceOption.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Commands
{
    public class DeleteServiceOptionCommand : IRequest<DeleteServiceOptionResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
