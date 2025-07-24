using Application.Appointment.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class GetAppoinmentCommand : IRequest<GetAppoinmentResponse>
    {
        [FromForm]
        public Guid? Id { get; set; }
        [FromForm]
        public string? UserId { get; set; }
        [FromForm]
        public string? Status { get; set; }
        [FromForm]
        public string? ArtistId { get; set; }
    }
}
