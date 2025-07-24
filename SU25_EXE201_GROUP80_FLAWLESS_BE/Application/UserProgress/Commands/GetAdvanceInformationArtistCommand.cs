using Application.UserProgress.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Commands
{
    public class GetAdvanceInformationArtistCommand : IRequest<GetAdvanceInformationArtistResponse>
    {
        [Required]
        public string ArtistId { get; set; }
    }
}
