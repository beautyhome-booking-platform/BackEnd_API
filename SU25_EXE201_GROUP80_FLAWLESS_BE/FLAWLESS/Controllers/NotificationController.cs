using Application.Notification.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ISender _mediator;
        public NotificationController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromForm] SendNotificationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-notification")]
        public async Task<IActionResult> GetNotification([FromForm] GetNotificationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Customer,Artist")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromForm] UpdateStatusCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteNotification([FromForm] DeleteNotificationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
    } 
}
