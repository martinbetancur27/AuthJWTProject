using Core.DTO.User;
using Core.DTO.UserDTO;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Moq;
using Xunit;

namespace AuthReviewProject.Tests.UnitTests.Core.Services
{
    public class UserServiceTest
    {
        Mock<IEncryptService> _mockEncryptService;
        Mock<IUserRepository> _mockUserRepository;
        Mock<IRoleService> _mockRoleService;

        public UserServiceTest()
        {
            _mockEncryptService = new Mock<IEncryptService>();
            _mockRoleService = new Mock<IRoleService>();
            _mockUserRepository = new Mock<IUserRepository>();
        }

        private CreateUserWithRoleDTO CreateDefaultUserWithRoleDTO()
        {
            return new CreateUserWithRoleDTO
            {
                Name = "TestName",
                UserName = "UsernameTest",
                Password = "123",
                idRole = 2
            };
        }

        private CreateUserDTO CreateDefaultUser()
        {
            return new CreateUserDTO
            {
                Name = "TestName",
                UserName = "UsernameTest",
                Password = "123"
            };
        }

        [Fact]
        public async Task AddUserWithRole_RoleNotExist_And_ReturnNotFound()
        {
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.AddWithRoleAsync(CreateDefaultUserWithRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AddUserWithRole_UsernameExist_And_ReturnForbidCode()
        {
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRepository.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.AddWithRoleAsync(CreateDefaultUserWithRoleDTO());

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddUserWithRole_UsernameExist_And_ReturnCreatedCode()
        {
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRepository.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);
            
            var result = await _userService.AddWithRoleAsync(CreateDefaultUserWithRoleDTO());

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task AddUser_UsernameExist_And_ReturnForbidCode()
        {
            _mockUserRepository.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.AddAsync(CreateDefaultUser());

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddUser_And_ReturnCreatedCode()
        {
            _mockUserRepository.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.AddAsync(CreateDefaultUser());

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_UserIdNotExist_And_ReturnNotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.DeleteByIdAsync(It.IsAny<int>());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_And_ReturnOK()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            var _userService = new UserService(_mockUserRepository.Object, _mockRoleService.Object, _mockEncryptService.Object);

            var result = await _userService.DeleteByIdAsync(It.IsAny<int>());

            Assert.Equal(200, result.StatusCode);
        }
    }
}
