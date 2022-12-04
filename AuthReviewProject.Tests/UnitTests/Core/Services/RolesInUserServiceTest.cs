using Core.DTO.User;
using Core.Entities.Auth;
using Core.Interfaces.Repositories;
using Core.Services;
using Moq;
using Xunit;

namespace AuthReviewProject.Tests.UnitTests.Core.Services
{
    public class RolesInUserServiceTest
    {
        Mock<IRoleRepository> _mockRoleRepository;
        Mock<IUserRepository> _mockUserRepository;
        Mock<IUserRoleRepository> _mockUserRoleRepository;

        public RolesInUserServiceTest()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRoleRepository = new Mock<IUserRoleRepository>();
        }

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

        [Fact]
        public async Task DeleteRoleInUser_UserIdNotExist_And_Return_NotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.DeleteAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRoleInUser_RoleIdNotExist_And_Return_NotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.DeleteAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRoleInUser_RoleNotExistInUser_And_Return_NotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleRepository.Setup(p => p.GetByUserAndRoleAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult((UserRole)null));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.DeleteAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRoleInUser_And_Return_OK()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleRepository.Setup(p => p.GetByUserAndRoleAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(CreateDefaultUserRole()));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.DeleteAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AddRoleInUser_UserIdNotExist_And_Return_NotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.AddAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AddRoleInUser_RoleIdNotExist_And_Return_NotFound()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(false));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.AddAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AddRoleInUser_RoleExistInUser_And_Return_ForbidCode()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleRepository.Setup(p => p.IsRoleAndUserAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult((true)));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.AddAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task AddRoleInUser_And_Return_CreatedCode()
        {
            _mockUserRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRoleRepository.Setup(p => p.IsIdRegisteredAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockUserRoleRepository.Setup(p => p.IsRoleAndUserAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(false));
            var _rolesInUserService = new RolesInUserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockUserRoleRepository.Object);

            var result = await _rolesInUserService.AddAsync(CreateDefaultUserRoleDTO());

            Assert.Equal(201, result.StatusCode);
        }
    }
}