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
                    return NotFound("Error System: The System can not validate if the user exists");
                }

                if (isUserInDatabase.Value)
                {
                    return BadRequest("Can not create user because it already exists");
                }

                int? newId = await _adminService.AddUserAndReturnIdAsync(user, createUser.idRole);

                if (newId == 0)
                {
                    return NotFound("Error System: The System can not add the user");
                }

                if (newId == null)
                {
                    return BadRequest("The user can not be created");
                }

                user.Id = newId.Value;

                return Ok("The user has been created");
            }

            return BadRequest("Insert all the flieds");
        }


        [Route("user/delete/{id:int}")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var responseDelete = await _adminService.DeleteUserAsync(id);

            if (responseDelete == null)
            {
                return NotFound("Error System: The System can not delete the user");
            }

            if (!responseDelete.Value)
            {
                return NotFound("Error: The user can not delete");
            }

            return Ok("The user has been deleted");
        }


        [Route("user/deleterole")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteRoleInUser([FromBody] UserRoleDTO deleteUserRoleOfUser)
        {
            if (ModelState.IsValid)
            {
                var responseDeleteRoleInUser = await _adminService.DeleteRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseDeleteRoleInUser == null)
                {
                    return NotFound("Error System: The System can not delete the role in the user");
                }

                if (!responseDeleteRoleInUser.Value)
                {
                    return NotFound("Error: The role of user can not delete");
                }

                return Ok("The role of user has been deleted");
            }

            return BadRequest("Insert all the flieds");
        }


        [Route("user/addrole")]
        [HttpPost()]
        public async Task<IActionResult> AddRoleInUser([FromBody] UserRoleDTO deleteUserRoleOfUser)
        {
            if (ModelState.IsValid)
            {
                var responseExistRoleInUser = await _adminService.ExistRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseExistRoleInUser == null)
                {
                    return NotFound("Error: System can not validate the role");
                }

                if (responseExistRoleInUser.Value)
                {
                    return Ok("Error: The user already has the role");
                }

                var responseDeleteRoleInUser = await _adminService.AddRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseDeleteRoleInUser == null)
                {
                    return NotFound("Error System: The System can not save the role in the user");
                }

                if (!responseDeleteRoleInUser.Value)
                {
                    return NotFound("Error: The role of user can not saved");
                }

                return Ok("The role of user has been saved");
            }

            return BadRequest("Insert all the flieds");
        }
    }
}
