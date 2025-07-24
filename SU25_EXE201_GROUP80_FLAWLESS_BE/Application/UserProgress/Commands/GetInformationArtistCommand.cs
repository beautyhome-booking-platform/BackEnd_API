using Application.UserProgress.Responses;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Commands
{
    public class GetInformationArtistCommand : IRequest<GetInformationArtistResponse>
    {
        public RequestStatus? RequestStatus { get; set; }
    }
}
