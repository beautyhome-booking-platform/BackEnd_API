using Application.BankInfo.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/bank-account")]
    [ApiController]
    public class BackInfoController : ControllerBase
    {
        ISender _mediator;
        public BackInfoController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("add-bank-account")]
        public async Task<IActionResult> AddBackAccount([FromBody] AddBackAccountCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-bank-account")]
        public async Task<IActionResult> GetBackAccounts([FromForm] GetBankAccountCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist")]
        [HttpPut("update-bank-account")]
        public async Task<IActionResult> UpdateBackAccount([FromBody] UpdateBankAccountCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist")]
        [HttpDelete("delete-bank-account")]
        public async Task<IActionResult> DeleteBackAccount([FromQuery] DeleteBankAccountCommand command)
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
