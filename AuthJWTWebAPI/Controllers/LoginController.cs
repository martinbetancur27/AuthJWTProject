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
        
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        public LoginController(ILoginService loginService, ITokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                ResponseLoginDTO responseLoginDTO = new ResponseLoginDTO();

                User? user = await _loginService.LoginAsync(userLogin.Username, userLogin.Password);

                if (user == null)
                {
                    responseLoginDTO.Result = 404;
                    responseLoginDTO.Mesagge = "User not found";
                    return NotFound(responseLoginDTO);
                }

                var token = _tokenService.GenerateToken(user);

                if (token == null)
                {
                    responseLoginDTO.Result = 404;
                    responseLoginDTO.Mesagge = "Can not create token";

                    return NotFound(responseLoginDTO);
                }

                responseLoginDTO.Result = 200;
                responseLoginDTO.Mesagge = "Succes";
                responseLoginDTO.ExpireInMinutes = TokenConstants.ExpireInMinutes;
                responseLoginDTO.Token = token;

                return Ok(responseLoginDTO);
            }

            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO
            {
                StatusCode = 500,
                Message = "Insert all the fields"
            };

            return BadRequest(responseGeneralDTO);
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
                    responseGeneralDTO.StatusCode = 400;
                    responseGeneralDTO.Message = "New password dont match";

                    return BadRequest(responseGeneralDTO);
                }

                User? user = await _loginService.LoginAsync(changePasswordDTO.Username, changePasswordDTO.CurrentPassword);

                if (user == null)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "User not found";

                    return NotFound(responseGeneralDTO);
                }

                _loginService.ChangePassword(user, changePasswordDTO.NewPassword);

                responseGeneralDTO.StatusCode = 201;
                responseGeneralDTO.Message = "Password changed";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.StatusCode = 400;
            responseGeneralDTO.Message = "Insert all the fields";

            return BadRequest(responseGeneralDTO);
        }
    }
}
