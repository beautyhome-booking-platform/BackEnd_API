using Application.Area.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/area")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        ISender _mediatR;
        public AreaController(ISender mediatR)
        {
            _mediatR = mediatR;
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpGet("get-city")]
        public async Task<IActionResult> GetCity()
        {
            var command = new GetCityCommand();
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPost("get-districts")]
        public async Task<IActionResult> GetDistrict(GetDistrictsCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    } 
}
