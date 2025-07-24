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
    public class SearchArtistCommand : IRequest<SearchArtistResponse>
    {
        public RequestStatus? RequestStatus { get; set; }
        public string? SearchContent { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
