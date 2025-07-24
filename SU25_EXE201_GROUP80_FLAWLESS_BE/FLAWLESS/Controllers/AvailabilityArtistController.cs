using Application.ArtistAvailability.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/schedule-artist")]
    [ApiController]
    public class AvailabilityArtistController : ControllerBase
    {
        private readonly ISender _mediatR;
        public AvailabilityArtistController(ISender mediatR)
        {
            _mediatR = mediatR;
        }
        [Authorize(Roles = "Artist")]
        [HttpPut("update-availability")]
        public async Task<IActionResult> SetAvailability([FromBody] UpdateAvailabilityCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-slot-time")]
        public async Task<IActionResult> GetSlotTime([FromBody] GetAvailabilityCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpPost("get-schedule")]
        public async Task<IActionResult> GetSchedule([FromBody] GetScheduleCommand command)
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
