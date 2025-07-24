using Application.Voucher.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly ISender _mediator;
        public VoucherController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpPost("create-voucher")]
        public async Task<IActionResult> CreateVoucher([FromForm] CreateVoucherCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-voucher")]
        public async Task<IActionResult> GetVoucher([FromForm] GetVoucherCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpPut("update-voucher")]
        public async Task<IActionResult> UpdateVoucher([FromForm] UpdateVoucherCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.ErrorMessage);
        }
        [Authorize(Roles = "Artist,Admin")]
        [HttpDelete("delete-voucher")]
        public async Task<IActionResult> DeleteVoucher([FromQuery] DeleteVoucherCommand command)
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
