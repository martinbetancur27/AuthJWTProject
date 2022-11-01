using Core.Constants;
using Core.DTO.Auth;
using Core.DTO.Response;
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
        private readonly ITokenService _tokenService;

        public LoginController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                ResponseLoginDTO responseLoginDTO = new ResponseLoginDTO();

                User? user = await _userService.GetUserAsync(userLogin.Username, userLogin.Password);

                if (user == null)
                {
                    responseLoginDTO.Result = 0;
                    responseLoginDTO.Mesagge = "User not found";
                    return NotFound(responseLoginDTO);
                }

                var token = _tokenService.GenerateToken(user);

                if (token == null)
                {
                    responseLoginDTO.Result = 0;
                    responseLoginDTO.Mesagge = "Can not create token";
                    return NotFound(responseLoginDTO);
                }

                responseLoginDTO.Result = 1;
                responseLoginDTO.Mesagge = "Succes";
                responseLoginDTO.ExpireInMinutes = TokenConstants.ExpireInMinutes;
                responseLoginDTO.Token = token;

                return Ok(responseLoginDTO);
            }

            return BadRequest("Insert all the flieds");
        }


        [AllowAnonymous]
        [HttpPut("user/changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                if (changePasswordDTO.NewPassword != changePasswordDTO.NewPasswordAgain)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "New password dont match";

                    return BadRequest(responseGeneralDTO);
                }

                User? user = await _userService.GetUserAsync(changePasswordDTO.Username, changePasswordDTO.CurrentPassword);

                if (user == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "User not found";

                    return NotFound(responseGeneralDTO);
                }

                user.Password = EncryptTool.GetSHA256OfString(changePasswordDTO.NewPassword);

                var responseChangePassword = _userService.ChangePassword(user);

                if (responseChangePassword == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "System can not change password";

                    return NotFound(responseGeneralDTO);
                }

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "Password changed";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }
    }
}
