using Application.Dashboard.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ISender _mediator;

        public DashboardController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("tota-revenue")]
        public async Task<IActionResult> TotalRevenue()
        {
            var command = new TotalRevenueCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("total-booking")]
        public async Task<IActionResult> TotalBooking()
        {
            var command = new TotalBookingCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }

        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-best-artist")]
        public async Task<IActionResult> GetBestArtist()
        {
            var command = new GetBestArtistCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-best-service")]
        public async Task<IActionResult> GetBestService()
        {
            var command = new GetBestServiceCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-total-artist")]
        public async Task<IActionResult> GetTotalArtist()
        {
            var command = new GetTotalArtistCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-total-customer")]
        public async Task<IActionResult> GetTotalCustomer()
        {
            var command = new GetTotalCustomerCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
