using AuthJWTWebAPI.Controllers;
using Core.DTO.User;
using Core.DTO.UserDTO;
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
    public class UsersControllerTest
    {
        Mock<IUserService> _mockUserService;
        Mock<IRoleService> _mockRoleService;
        Mock<IRolesInUserService> _mockUserRoleService;

        public UsersControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _mockRoleService = new Mock<IRoleService>();
            _mockUserRoleService = new Mock<IRolesInUserService>();
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
            _mockUserService.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.AddUserWithRole(CreateDefaultUserWithRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddUserWithRole_UsernameExist_And_ReturnForbidCode()
        {
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserService.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.AddUserWithRole(CreateDefaultUserWithRoleDTO()) as ObjectResult;

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddUserWithRole_UsernameExist_And_ReturnCreatedCode()
        {
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserService.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);
            var result = await _userController.AddUserWithRole(CreateDefaultUserWithRoleDTO()) as ObjectResult;

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task AddUser_UsernameExist_And_ReturnForbidCode()
        {
            _mockUserService.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.AddUser(CreateDefaultUser()) as ObjectResult;

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddUser_And_ReturnCreatedCode()
        {
            _mockUserService.Setup(p => p.IsUsernameRegisteredAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.AddUser(CreateDefaultUser()) as ObjectResult;

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_UserIdNotExist_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.DeleteUser(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_And_ReturnOK()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            var _userController = new UsersController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userController.DeleteUser(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
