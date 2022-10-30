using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Core.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (ModelState.IsValid)
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

            return BadRequest("Insert all the flieds");
        }

        [AllowAnonymous]
        [Route("user/changepassword")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (ModelState.IsValid)
            {

                if (changePasswordDTO.NewPassword != changePasswordDTO.NewPasswordAgain)
                {
                    return BadRequest("New password dont match");
                }

                User? user = await _userService.GetUserAsync(changePasswordDTO.Username, changePasswordDTO.CurrentPassword);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.Password = EncryptTool.GetSHA256OfString(changePasswordDTO.NewPassword);

                var responseChangePassword = _userService.ChangePassword(user);

                if (responseChangePassword == null)
                {
                    return NotFound("System can not change password");
                }

                return Ok("Succes: Change password");
            }

            return BadRequest("Insert all the flieds");
        }
    }
}
