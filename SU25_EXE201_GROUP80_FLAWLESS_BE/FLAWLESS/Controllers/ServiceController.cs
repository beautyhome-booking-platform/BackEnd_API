using Application.Services.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ISender _mediatR;
        public ServiceController(ISender mediatR)
        {
            _mediatR = mediatR;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-new-service")]
        public async Task<IActionResult> CreateService([FromForm] CreateServiceCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin,Customer,Artist")]
        [HttpPost("get-service")]
        public async Task<IActionResult> GetService([FromForm] GetServiceCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-service")]
        public async Task<IActionResult> UpdateService([FromForm] UpdateServiceCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-service")]
        public async Task<IActionResult> DeleteService([FromForm] DeleteServiceCommand command)
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
