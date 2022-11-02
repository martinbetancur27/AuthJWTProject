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
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost("user/addwithrole")]
        public async Task<IActionResult> AddUserWithRole(CreateUserWithRole createUserWithRole)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isRoleInDatabase = await _adminService.IsRoleInDatabaseAsync(createUserWithRole.idRole);

                if (!isRoleInDatabase)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The id role does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isUserInDatabase = await _adminService.IsUserInDatabaseByUsernameAsync(createUserWithRole.UserName);

                if (isUserInDatabase)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Can not create user because username already exists";

                    return BadRequest(responseGeneralDTO);
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

                await _adminService.AddUserWithRoleAndReturnIdUserAsync(user, userRole);

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The user has been created";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }

        
        [HttpPost("user/add")]
        public async Task<IActionResult> AddUser(CreateUserDTO createUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isUserInDatabase = await _adminService.IsUserInDatabaseByUsernameAsync(createUser.UserName);

                if (isUserInDatabase)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Can not create user because username already exists";

                    return BadRequest(responseGeneralDTO);
                }

                User user = new User
                {
                    Name = createUser.Name,
                    UserName = createUser.UserName,
                    Password = createUser.Password
                };

                await _adminService.AddUserAndReturnIdAsync(user);

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The user has been created";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }


        [HttpDelete("user/deletebyid/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var isUserInDatabase = await _adminService.IsUserInDatabaseByIdAsync(id);

            if (!isUserInDatabase)
            {
                responseGeneralDTO.Result = 0;
                responseGeneralDTO.Mesagge = "Error: user id does not exist";

                return NotFound(responseGeneralDTO);
            }

            await _adminService.DeleteUserAsync(id);

            responseGeneralDTO.Result = 1;
            responseGeneralDTO.Mesagge = "The user has been deleted";

            return Ok(responseGeneralDTO);
        }


        [HttpDelete("user/deleterole")]
        public async Task<IActionResult> DeleteRoleInUser([FromBody] UserRoleDTO deleteRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var roleInUserFromDb = await _adminService.GetUserRoleDatabaseAsync(deleteRoleInUser.IdUser, deleteRoleInUser.IdRole);

                if (roleInUserFromDb == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The user does not have that role or not exists";

                    return BadRequest(responseGeneralDTO);
                }

                await _adminService.DeleteRoleInUserAsync(roleInUserFromDb);

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The role of user has been deleted";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }


        [HttpPost("user/addrole")]
        public async Task<IActionResult> AddRoleInUser([FromBody] UserRoleDTO newRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var isUserInDatabase = await _adminService.IsUserInDatabaseByIdAsync(newRoleInUser.IdUser);

                if (!isUserInDatabase)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The id user does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isRoleInDatabase = await _adminService.IsRoleInDatabaseAsync(newRoleInUser.IdRole);

                if (!isRoleInDatabase)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The id role user does not exist";

                    return NotFound(responseGeneralDTO);
                }

                var isRoleInUserFromDb = await _adminService.IsRoleInUserAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                if (isRoleInUserFromDb)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The user already has that role";

                    return NotFound(responseGeneralDTO);
                }

                await _adminService.AddRoleInUserAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The role in the user has been saved";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }
    }
}
