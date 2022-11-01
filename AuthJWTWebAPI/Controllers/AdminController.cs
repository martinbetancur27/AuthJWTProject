using Core.DTO.Response;
using Core.DTO.User;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost("user/add")]
        public async Task<IActionResult> AddUser(CreateUserDTO createUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = createUser.Name,
                    UserName = createUser.UserName,
                    Password = createUser.Password
                };

                var isUserInDatabase = await _adminService.IsUserInDatabaseAsync(createUser.UserName);

                if (isUserInDatabase == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The System can not validate if the user exists";

                    return NotFound(responseGeneralDTO);
                }

                if (isUserInDatabase.Value)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Can not create user because it already exists";

                    return BadRequest(responseGeneralDTO);
                }

                int? newId = await _adminService.AddUserAndReturnIdAsync(user, createUser.idRole);

                if (newId == 0)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The System can not add the user";
                    return NotFound(responseGeneralDTO);
                }

                if (newId == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "The user can not be created";

                    return BadRequest(responseGeneralDTO);
                }

                user.Id = newId.Value;

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The user has been created";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }


        [HttpDelete("user/delete/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var responseDelete = await _adminService.DeleteUserAsync(id);

            if (responseDelete == null)
            {
                responseGeneralDTO.Result = 0;
                responseGeneralDTO.Mesagge = "Error System: The System can not delete the user";

                return NotFound(responseGeneralDTO);
            }

            if (!responseDelete.Value)
            {
                responseGeneralDTO.Result = 0;
                responseGeneralDTO.Mesagge = "Error: The user can not delete";

                return NotFound(responseGeneralDTO);
            }

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
                    responseGeneralDTO.Mesagge = "Error: The user does not have that role";

                    return BadRequest(responseGeneralDTO);
                }

                var responseDeleteRoleInUser = await _adminService.DeleteRoleInUserAsync(roleInUserFromDb);

                if (responseDeleteRoleInUser == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The role in the user can not delete";

                    NotFound(responseGeneralDTO);
                }

                if (!responseDeleteRoleInUser.Value)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The role of user can not delete";

                    return NotFound(responseGeneralDTO);
                }

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
                var roleInUserFromDb = await _adminService.GetUserRoleDatabaseAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                if (roleInUserFromDb != null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The user already has that role";

                    return NotFound(responseGeneralDTO);
                }

                var responseAddRoleInUser = await _adminService.AddRoleInUserAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

                if (responseAddRoleInUser == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The System can not save the role in the user";

                    return NotFound(responseGeneralDTO);
                }

                if (!responseAddRoleInUser.Value)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The role in the user can not saved";

                    return NotFound(responseGeneralDTO);
                }

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
