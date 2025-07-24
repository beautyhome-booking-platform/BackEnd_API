using Application.ServiceOption.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/service-option")]
    [ApiController]
    public class ServiceOptionController : ControllerBase
    {
        private readonly ISender _mediator;
        public ServiceOptionController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Artist")]
        [HttpPost("create-new-service-option")]
        public async Task<IActionResult> CreateServiceOption([FromForm] CreateServiceOptionCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-service-option")]
        public async Task<IActionResult> GetServiceOption([FromForm] GetServiceOptionCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Artist")]
        [HttpPut("update-service-option")]
        public async Task<IActionResult> UpdateServiceOption([FromForm] UpdateServiceOptionCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Artist")]
        [HttpDelete("delete-service-option")]
        public async Task<IActionResult> DeleteServiceOption([FromForm] DeleteServiceOptionCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
