using Application.Post.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ISender _mediator;
        public PostController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-post")]
        public async Task<IActionResult> GetPost([FromForm] GetPostCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpPut("update-post")]
        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpDelete("delete-post")]
        public async Task<IActionResult> DeletePost([FromForm] DeletePostCommand command)
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
