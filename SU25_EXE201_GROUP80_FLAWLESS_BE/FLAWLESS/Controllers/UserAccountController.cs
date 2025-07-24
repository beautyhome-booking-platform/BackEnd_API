using Application.UserAccount.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FLAWLESS.Controllers
{
    [Route("api/user-account")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly ISender _mediatR;

        public UserAccountController(ISender mediatR)
        {
            _mediatR = mediatR;
        }

        
        [HttpGet("protected")]
        public IActionResult GetProtected()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            return Ok(new { message = "✅ Authorized", email, name });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromForm] RegistrationCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] LoginGoogleCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.TokenId))
                return BadRequest("TokenId is required.");

            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("register-artist")]
        public async Task<IActionResult> RegisterAsArtist( RegistrationArtistCommand command)
        {

            // Gọi Handler để xử lý đăng ký
            var response = await _mediatR.Send(command);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Accept-artist")]
        public async Task<IActionResult> AccpetArtist([FromForm] AcceptArtistCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("verify-twofactor-code")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] VerifyTwoFactorCodeCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize(Roles ="Customer,Artist,Admin")]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var response = await _mediatR.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Customer,Artist,Admin")]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileCommand command)
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
