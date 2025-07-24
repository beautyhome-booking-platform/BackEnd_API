using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FLAWLESS.Controllers
{
    [Route("api/artist-progress")]
    [ApiController]
    public class ArtistProgressController : ControllerBase
    {
        private readonly ISender _mediatR;

        public ArtistProgressController(ISender mediatR)
        {
            _mediatR = mediatR;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("search-artist")]
        public async Task<IActionResult> SearchArtist([FromForm] SearchArtistCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response); 
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-artist-progress")]
        public async Task<IActionResult> GetArtistProgress([FromForm] GetArtistProgressCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-artist-advanced-information")]
        public async Task<IActionResult> GetAdvanceInformationArtist([FromForm] GetAdvanceInformationArtistCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
