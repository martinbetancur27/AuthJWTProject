using AuthJWTWebAPI.Controllers;
using Core.DTO.User;
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
    public class UserRolesControllerTest
    {
        Mock<IUserService> _mockUserService;
        Mock<IRoleService> _mockRoleService;
        Mock<IRolesInUserService> _mockUserRoleService;

        private UserRole CreateDefaultUserRole()
        {
            return new UserRole
            {
                Id = 1,
                IdRole = 1,
                IdUser = 1
            };
        }

        private UserRoleDTO CreateDefaultUserRoleDTO()
        {
            return new UserRoleDTO
            {
                IdRole = 1,
                IdUser = 1
            };
        }

        public UserRolesControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _mockRoleService = new Mock<IRoleService>();
            _mockUserRoleService = new Mock<IRolesInUserService>();
        }

        [Fact]
        public async Task DeleteRoleInUser_UserIdNotExist_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.DeleteRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRoleInUser_RoleIdNotExist_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.DeleteRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRoleInUser_RoleNotExistInUser_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleService.Setup(p => p.GetByUserAndRoleAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult((UserRole)null));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.DeleteRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRoleInUser_And_ReturnOK()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleService.Setup(p => p.GetByUserAndRoleAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(CreateDefaultUserRole()));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.DeleteRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddRoleInUser_UserIdNotExist_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.AddRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddRoleInUser_RoleIdNotExist_And_ReturnNotFound()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.AddRoleInUser(CreateDefaultUserRoleDTO());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddRoleInUser_RoleExistInUser_And_ReturnForbidCode()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleService.Setup(p => p.IsRoleAndUserAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult((true)));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.AddRoleInUser(CreateDefaultUserRoleDTO()) as ObjectResult;

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddRoleInUser_And_ReturnCreatedCode()
        {
            _mockUserService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleService.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleService.Setup(p => p.IsRoleAndUserAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(false));
            var _userRolesController = new UserRolesController(_mockUserService.Object, _mockRoleService.Object, _mockUserRoleService.Object);

            var result = await _userRolesController.AddRoleInUser(CreateDefaultUserRoleDTO()) as ObjectResult;

            Assert.Equal(201, result.StatusCode);
        }
    }
}
