using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public LoginController( IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            User? user = await _userService.GetUserAsync(userLogin.Username, userLogin.Password);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var token = _userService.GenerateToken(user);

            if (token == null)
            {
                return NotFound("System can not create token");
            }

            return Ok(token);
        }
    }
}
