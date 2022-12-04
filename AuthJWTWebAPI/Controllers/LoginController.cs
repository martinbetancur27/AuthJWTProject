using Core.DTO.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var responseLoginDTO = await _loginService.LoginAsync(userLogin);

            return new ObjectResult(responseLoginDTO) { StatusCode = responseLoginDTO.StatusCode };
        }

        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var responseGeneralDTO = await _loginService.ChangePasswordAsync(changePasswordDTO);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO?.StatusCode };
        }
    }
}