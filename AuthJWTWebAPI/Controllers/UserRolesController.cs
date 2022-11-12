using Core.DTO.Response;
using Core.DTO.User;
using Core.DTO.UserDTO;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthJWTWebAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRolesInUserService _rolesInUserService;

        public UserRolesController(IUserService userService, IRoleService roleService, IRolesInUserService rolesInUserService)
        {
            _userService = userService;
            _roleService = roleService;
            _rolesInUserService = rolesInUserService;
        }


        [HttpDelete()]
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


        [HttpPost()]
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
