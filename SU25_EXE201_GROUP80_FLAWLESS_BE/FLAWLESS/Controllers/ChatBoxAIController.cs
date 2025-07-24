using Application.UserService.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/chat-box-ai")]
    [ApiController]
    public class ChatBoxAIController : ControllerBase
    {
        private readonly ISender _mediator;

        public ChatBoxAIController(ISender mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskChatCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
