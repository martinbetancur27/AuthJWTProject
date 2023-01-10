using Core.DTO.Response;
using Core.DTO.UserDTO;
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
        private readonly IRolesInUserService _rolesInUserService;

        public UserRolesController(IRolesInUserService rolesInUserService)
        {
            _rolesInUserService = rolesInUserService;
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteRoleInUser([FromBody] UserRoleDTO deleteRoleInUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseGeneralDTO responseGeneralDTO = await _rolesInUserService.DeleteAsync(deleteRoleInUser);
            
            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }

        [HttpPost()]
        public async Task<IActionResult> AddRoleInUser([FromBody] UserRoleDTO newRoleInUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseGeneralDTO responseGeneralDTO = await _rolesInUserService.AddAsync(newRoleInUser);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }
    }
}