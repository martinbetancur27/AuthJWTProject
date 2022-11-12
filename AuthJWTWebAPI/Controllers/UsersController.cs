using Core.DTO.Response;
using Core.DTO.User;
using Core.DTO.UserDTO;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;

namespace AuthJWTWebAPI.Controllers
{

    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRolesInUserService _rolesInUserService;

        public UsersController(IUserService userService, IRoleService roleService, IRolesInUserService rolesInUserService)
        {
            _userService = userService;
            _roleService = roleService;
            _rolesInUserService = rolesInUserService;
        }

        
        [HttpPost()]
        public async Task<IActionResult> AddUser(CreateUserDTO createUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isUserInDatabase = await _userService.IsUsernameRegisteredAsync(createUser.UserName);

                if (isUserInDatabase)
                {
                    responseGeneralDTO.StatusCode = 403;
                    responseGeneralDTO.Message = "Username already exists";

                    return new ObjectResult(responseGeneralDTO) { StatusCode = 403 };
                }

                User user = new User
                {
                    Name = createUser.Name,
                    UserName = createUser.UserName,
                    Password = createUser.Password
                };

                await _userService.AddAndReturnIdAsync(user);

                responseGeneralDTO.StatusCode = 201;
                responseGeneralDTO.Message = "The user has been created";

                return new ObjectResult(responseGeneralDTO) { StatusCode = 201 };
            }

            responseGeneralDTO.StatusCode = 400;
            responseGeneralDTO.Message = "Insert all the fields";

            return BadRequest(responseGeneralDTO);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var isUserInDatabase = await _userService.IsIdRegisteredAsync(id);

            if (!isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "User Id does not exist";

                return NotFound(responseGeneralDTO);
            }

            await _userService.DeleteByIdAsync(id);

            responseGeneralDTO.StatusCode = 200;
            responseGeneralDTO.Message = "The user has been deleted";

            return Ok(responseGeneralDTO);
        }


        [HttpPost("withrole")]
        public async Task<IActionResult> AddUserWithRole(CreateUserWithRoleDTO createUserWithRole)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isRoleInDatabase = await _roleService.IsIdRegisteredAsync(createUserWithRole.idRole);

                if (!isRoleInDatabase)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "The id role does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isUserInDatabase = await _userService.IsUsernameRegisteredAsync(createUserWithRole.UserName);

                if (isUserInDatabase)
                {
                    responseGeneralDTO.StatusCode = 403;
                    responseGeneralDTO.Message = "Username already exists";

                    return new ObjectResult(responseGeneralDTO) { StatusCode = 403 };
                }

                UserRole userRole = new UserRole
                {
                    IdRole = createUserWithRole.idRole
                };

                User user = new User
                {
                    Name = createUserWithRole.Name,
                    UserName = createUserWithRole.UserName,
                    Password = createUserWithRole.Password
                };

                await _rolesInUserService.AddUserAndPutRoleAndReturnIdUserAsync(user, userRole);

                responseGeneralDTO.StatusCode = 201;
                responseGeneralDTO.Message = "The user has been created";

                return new ObjectResult(responseGeneralDTO) { StatusCode = 201 };
            }

            responseGeneralDTO.StatusCode = 400;
            responseGeneralDTO.Message = "Insert all the fields";

            return BadRequest(responseGeneralDTO);
        }
    }
}
