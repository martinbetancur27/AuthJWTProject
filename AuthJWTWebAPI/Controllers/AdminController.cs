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
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRolesInUserService _rolesInUserService;

        public AdminController(IUserService userService, IRoleService roleService, IRolesInUserService rolesInUserService)
        {
            _userService = userService;
            _roleService = roleService;
            _rolesInUserService = rolesInUserService;
        }


        [HttpPost("user/addwithrole")]
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

                    return new ObjectResult(responseGeneralDTO) { StatusCode = 403};
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

        
        [HttpPost("user/add")]
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


        [HttpDelete("user/deletebyid/{id:int}")]
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


        [HttpDelete("user/deleterole")]
        public async Task<IActionResult> DeleteRoleInUser([FromBody] UserRoleDTO deleteRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isUserInDatabase = await _userService.IsIdRegisteredAsync(deleteRoleInUser.IdUser);

                if (!isUserInDatabase)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "User Id does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isRoleInDatabase = await _roleService.IsIdRegisteredAsync(deleteRoleInUser.IdRole);

                if (!isRoleInDatabase)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "Role Id does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var roleInUserFromDb = await _rolesInUserService.GetByUserAndRoleAsync(deleteRoleInUser.IdUser, deleteRoleInUser.IdRole);

                if (roleInUserFromDb == null)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "User does not have that role";

                    return NotFound(responseGeneralDTO);
                }

                await _rolesInUserService.DeleteAsync(roleInUserFromDb);

                responseGeneralDTO.StatusCode = 200;
                responseGeneralDTO.Message = "Role removed in the user";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.StatusCode = 400;
            responseGeneralDTO.Message = "Insert all the fields";

            return BadRequest(responseGeneralDTO);
        }


        [HttpPost("user/addrole")]
        public async Task<IActionResult> AddRoleInUser([FromBody] UserRoleDTO newRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isUserInDatabase = await _userService.IsIdRegisteredAsync(newRoleInUser.IdUser);

                if (!isUserInDatabase)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "User Id does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isRoleInDatabase = await _roleService.IsIdRegisteredAsync(newRoleInUser.IdRole);

                if (!isRoleInDatabase)
                {
                    responseGeneralDTO.StatusCode = 404;
                    responseGeneralDTO.Message = "Role Id does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isRoleInUserFromDb = await _rolesInUserService.IsRoleAndUserAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                if (isRoleInUserFromDb)
                {
                    responseGeneralDTO.StatusCode = 403;
                    responseGeneralDTO.Message = "User already has that role";

                    return new ObjectResult(responseGeneralDTO) { StatusCode = 403 };
                }

                await _rolesInUserService.AddAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                responseGeneralDTO.StatusCode = 201;
                responseGeneralDTO.Message = "Role saved in the user";

                return new ObjectResult(responseGeneralDTO) { StatusCode = 201 };
            }

            responseGeneralDTO.StatusCode = 400;
            responseGeneralDTO.Message = "Insert all the fields";

            return BadRequest(responseGeneralDTO);
        }
    }
}
