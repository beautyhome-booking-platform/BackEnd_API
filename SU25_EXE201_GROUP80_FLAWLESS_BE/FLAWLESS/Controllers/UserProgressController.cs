using Application.UserProgress.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly ISender _mediatR;
        public UserProgressController(ISender mediatR)
        {
            _mediatR = mediatR;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-user")]
        public async Task<IActionResult> GetUser([FromForm] GetUserCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-information-customer")]
        public async Task<IActionResult> GetInformationCustomer()
        {
            var command = new GetInformationCustomerCommand();
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-information-artist")]
        public async Task<IActionResult> GetInformationArtist()
        {
            var command = new GetInformationArtistCommand();
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
    