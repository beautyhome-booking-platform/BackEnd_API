using Application.Appointment.Commands;
using Application.Transaction.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        ISender _mediator;
        public TransactionController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-transaction")]
        public async Task<IActionResult> GetTransaction([FromForm] GetTransactionCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("confirm-refund")]
        public async Task<IActionResult> ArtistCancel([FromForm] ConfirmRefundCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("pay-out-artist")]
        public async Task<IActionResult> PayoutArtist([FromForm] PayoutArtitstCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
