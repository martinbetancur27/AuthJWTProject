using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Core.Services;
using Moq;
using Xunit;

namespace AuthReviewProject.Tests.UnitTests.Core.Services
{
    public class LoginServiceTest
    {
        Mock<IEncryptService> _mockEncryptService;
        Mock<IUserService> _mockUserService;
        Mock<ITokenService> _mockTokenService;

        public LoginServiceTest()
        {
            _mockEncryptService = new Mock<IEncryptService>();
            _mockUserService = new Mock<IUserService>();
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
        public async Task Login_UserNotExist_And_Return_NotFound()
        {            
            _mockUserService.Setup(p => p.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((User)null));
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.LoginAsync(CreateDefaultLoginUserDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Login_TokenNotDone_And_Return_InternalError()
        {
            _mockUserService.Setup(p => p.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            _mockTokenService.Setup(p => p.GenerateToken(CreateDefaultUser())).Returns((string)null);
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.LoginAsync(CreateDefaultLoginUserDTO());

            Assert.Equal(500, result.StatusCode);
        }

                //Dont Work
        /*[Fact]
        public async Task Login_Success_And_ReturnOK()
        {
            _mockUserService.Setup(p => p.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            _mockTokenService.Setup(p => p. GenerateToken(CreateDefaultUser())).Returns(It.IsAny<string>());
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.LoginAsync(CreateDefaultLoginUserDTO());

            Assert.Equal(200, result.StatusCode);
        }*/

        [Fact]
        public async Task ChangePassword_NewPasswordsDontMatch_And_Return_BadRequest()
        {
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.ChangePasswordAsync(CreateDefaultChangePasswordDTOWithNewPassWordsDifferent());

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_UserNotExist_And_Return_NotFound()
        {
            _mockUserService.Setup(p => p.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((User)null));
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.ChangePasswordAsync(CreateDefaultChangePasswordDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_Success_And_Return_OK()
        {
            _mockUserService.Setup(p => p.FindByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(CreateDefaultUser()));
            var _loginService = new LoginService(_mockUserService.Object, _mockEncryptService.Object, _mockTokenService.Object);

            var result = await _loginService.ChangePasswordAsync(CreateDefaultChangePasswordDTO());

            Assert.Equal(201, result.StatusCode);
        }
    }
}
