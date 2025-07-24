using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Responses
{
    public class SearchArtistResponse : BaseResponse
    {
        public List<ArtistDTO> Artists { get; set; } = new List<ArtistDTO>();
        public int TotalCount { get; set; }
    }
}
