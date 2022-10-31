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


        [Route("user/delete/{id:int}")]
        [HttpDelete()]
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


        [Route("user/deleterole")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteRoleInUser([FromBody] UserRoleDTO deleteUserRoleOfUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var responseDeleteRoleInUser = await _adminService.DeleteRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseDeleteRoleInUser == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The System can not delete the role in the user";

                    return NotFound(responseGeneralDTO);
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


        [Route("user/addrole")]
        [HttpPost()]
        public async Task<IActionResult> AddRoleInUser([FromBody] UserRoleDTO deleteUserRoleOfUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (ModelState.IsValid)
            {
                var responseExistRoleInUser = await _adminService.ExistRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseExistRoleInUser == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: System can not validate the role";

                    return NotFound(responseGeneralDTO);
                }

                if (responseExistRoleInUser.Value)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The user already has the role";

                    return Ok(responseGeneralDTO);
                }

                var responseDeleteRoleInUser = await _adminService.AddRoleInUserAsync(deleteUserRoleOfUser.IdUser, deleteUserRoleOfUser.IdRole);

                if (responseDeleteRoleInUser == null)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error System: The System can not save the role in the user";

                    return NotFound(responseGeneralDTO);
                }

                if (!responseDeleteRoleInUser.Value)
                {
                    responseGeneralDTO.Result = 0;
                    responseGeneralDTO.Mesagge = "Error: The role of user can not saved";

                    return NotFound(responseGeneralDTO);
                }

                responseGeneralDTO.Result = 1;
                responseGeneralDTO.Mesagge = "The role of user has been saved";

                return Ok(responseGeneralDTO);
            }

            responseGeneralDTO.Result = 0;
            responseGeneralDTO.Mesagge = "Insert all the flieds";

            return BadRequest(responseGeneralDTO);
        }
    }
}
