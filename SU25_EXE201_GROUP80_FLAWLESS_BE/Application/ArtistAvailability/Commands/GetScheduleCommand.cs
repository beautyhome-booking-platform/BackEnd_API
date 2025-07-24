using Application.ArtistAvailability.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ArtistAvailability.Commands
{
    public class GetScheduleCommand : IRequest<GetScheduleResponse>
    {
        [Required]
        public string ArtistId { get; set; }
    }
}
