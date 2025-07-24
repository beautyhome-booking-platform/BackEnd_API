using Application.Conversation.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/conversation")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly ISender _mediator;
        public ConversationController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-list-conversation")]
        public async Task<IActionResult> GetListConversation()
        {
            var response = await _mediator.Send(new Application.Conversation.Commands.GetListChatCommand());
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-message")]
        public async Task<IActionResult> GetMessage([FromForm] GetMessageCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPut("readed-message")]
        public async Task<IActionResult> ReadedMessage([FromForm] ReadedMessageCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpDelete("delete-message")]
        public async Task<IActionResult> DeleteConversation([FromForm] DeleteMessageCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpDelete("delete-convarsation")]
        public async Task<IActionResult> DeleteConversation([FromForm] DeleteConversationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
