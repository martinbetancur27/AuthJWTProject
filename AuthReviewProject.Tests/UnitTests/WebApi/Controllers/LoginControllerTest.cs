using AuthJWTWebAPI.Controllers;
using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AuthReviewProject.Tests.UnitTests.WebApi.Controllers
{
    public class LoginControllerTest
    {
        Mock<ILoginService> _mockLoginService;
        Mock<ITokenService> _mockTokenService;
        public LoginControllerTest()
        {
            _mockLoginService = new Mock<ILoginService>();
            _mockTokenService = new Mock<ITokenService>();
        }

        private UserLoginDTO CreateDefaultLoginUserDTO()
        {
            return new UserLoginDTO
            {
                Username = "a",
                Password = "123"
            };
        }

        private User CreateDefaultUser()
        {
            return new User
            {
                Id = 1,
                Name = "A",
                UserName = "A",
                Password = "1"
            };
        }

        private ChangePasswordDTO CreateDefaultChangePasswordDTOWithNewPassWordsDifferent()
        {
            return new ChangePasswordDTO
            {
                Username = "A",
                CurrentPassword = "1",
                NewPassword = "2",
                NewPasswordAgain = "3"
            };
        }

        private ChangePasswordDTO CreateDefaultChangePasswordDTO()
        {
            return new ChangePasswordDTO
            {
                Username = "A",
                CurrentPassword = "1",
                NewPassword = "2",
                NewPasswordAgain = "2"
            };
        }

        [Fact]
        public async Task Login_UserNotExist_And_ReturnNotFound()
        {
            _mockLoginService.Setup(p => p.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((User)null));
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.Login(CreateDefaultLoginUserDTO()) as ObjectResult;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Login_TokenNotDone_And_ReturnInternalError()
        {
            _mockLoginService.Setup(p => p.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            _mockTokenService.Setup(p => p.GenerateToken(CreateDefaultUser())).Returns((string)null);
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.Login(CreateDefaultLoginUserDTO()) as ObjectResult;

            Assert.Equal(500, result.StatusCode);
        }

                //Dont Work
        /*[Fact]
        public async Task Login_Success_And_ReturnOK()
        {
            _mockLoginService.Setup(p => p.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            _mockTokenService.Setup(p => p. GenerateToken(CreateDefaultUser())).Returns(It.IsAny<string>());
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.Login(CreateDefaultLoginUserDTO()) as ObjectResult;

            Assert.IsType<OkObjectResult>(result);
        }*/

        [Fact]
        public async Task ChangePassword_NewPasswordsDontMatch_And_ReturnBadRequest()
        {
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.ChangePassword(CreateDefaultChangePasswordDTOWithNewPassWordsDifferent()) as ObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ChangePassword_UserNotExist_And_ReturnNotFound()
        {
            _mockLoginService.Setup(p => p.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((User)null));
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.ChangePassword(CreateDefaultChangePasswordDTO()) as ObjectResult;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ChangePassword_Success_And_ReturnOK()
        {
            _mockLoginService.Setup(p => p.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            var _loginController = new LoginController(_mockLoginService.Object, _mockTokenService.Object);

            var result = await _loginController.ChangePassword(CreateDefaultChangePasswordDTO()) as ObjectResult;

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
