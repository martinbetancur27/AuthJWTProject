using Core.Constants;
using Core.DTO.Auth;
using Core.DTO.Response;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService; 
        private readonly IEncryptService _encryptService;
        private readonly ITokenService _tokenService;

        public LoginService(IUserService userService, IEncryptService encryptService, ITokenService tokenService)
        {
            _encryptService = encryptService;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<ResponseLoginDTO> LoginAsync(UserLoginDTO userLogin)
        {
            ResponseLoginDTO responseLoginDTO = new ResponseLoginDTO();
            
            User? user = await _userService.FindByUsernameAndPasswordAsync(userLogin.Username, userLogin.Password);

            if (user == null)
                {
                    responseLoginDTO.StatusCode = 404;
                    responseLoginDTO.Message = "Wrong login";

                    return responseLoginDTO;
                }

                var token = _tokenService.GenerateToken(user);

                if (token == null)
                {
                    responseLoginDTO.StatusCode = 500;
                    responseLoginDTO.Message = "System can not create token";

                    return responseLoginDTO;
                }

                responseLoginDTO.StatusCode = 200;
                responseLoginDTO.Message = "Succes";
                responseLoginDTO.ExpireInMinutes = TokenConstants.ExpireInMinutes;
                responseLoginDTO.Token = token;

                return responseLoginDTO;
        }

        public async Task<ResponseGeneralDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (changePasswordDTO.NewPassword != changePasswordDTO.NewPasswordAgain)
                {
                    responseGeneralDTO.StatusCode = 400;
                    responseGeneralDTO.Message = "New password dont match";

                    return responseGeneralDTO;
                }

                User? user = await _userService.FindByUsernameAndPasswordAsync(changePasswordDTO.Username, changePasswordDTO.CurrentPassword);

                if (user == null)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "User not found";

                    return responseGeneralDTO;
                }

                user.Password = _encryptService.GetSHA256OfString(changePasswordDTO.NewPassword);
                _userService.Update(user);

                responseGeneralDTO.StatusCode = 201;
                responseGeneralDTO.Message = "Password changed";

                return responseGeneralDTO;
        }
    }
}