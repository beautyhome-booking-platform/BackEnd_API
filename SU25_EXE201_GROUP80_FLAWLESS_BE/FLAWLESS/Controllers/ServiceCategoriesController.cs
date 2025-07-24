using Application.ServiceCategories.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLAWLESS.Controllers
{
    [Route("api/service-category")]
    [ApiController]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly ISender _mediator;
        public ServiceCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-new-service-category")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles ="Admin,Customer,Artist")]
        [HttpPost("get-service-category")]
        public async Task<IActionResult> GetCategory([FromForm] GetCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-service-category")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-service-category")]
        public async Task<IActionResult> DeleteCategory([FromForm] DeleteCategoryCommand command)
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
