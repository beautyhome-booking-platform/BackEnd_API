using Application.Feedback.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ISender _mediator;
        public FeedbackController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedback([FromForm] CreateFeedbackCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-feedback")]
        public async Task<IActionResult> GetFeedback( GetFeedbackCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-list-feedback")]
        public async Task<IActionResult> GetListFeedback()
        {
            var command = new GetListFeedbackCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer")]
        [HttpPut("update-feedback")]
        public async Task<IActionResult> UpdateFeedback([FromForm] UpdateFeedbackCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("delete-feedback")]
        public async Task<IActionResult> DeleteFeedback([FromForm] DeleteFeedbackCommand command)
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
