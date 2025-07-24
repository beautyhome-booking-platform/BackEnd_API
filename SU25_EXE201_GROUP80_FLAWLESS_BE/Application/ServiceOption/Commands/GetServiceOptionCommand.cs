using Application.ServiceOption.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Commands
{
    public class GetServiceOptionCommand : IRequest<GetServiceOptionReponse>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? ServiceId { get; set; }
        public string? ArtistId { get; set; }
    }
}
