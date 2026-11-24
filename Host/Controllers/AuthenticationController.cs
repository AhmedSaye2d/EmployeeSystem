using Application.Dtos.AccountDto;
using Application.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccount _accountService;

        public AuthenticationController(IUserAccount accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.CreateAsync(register);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.LoginAsync(login);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }
    }
}
