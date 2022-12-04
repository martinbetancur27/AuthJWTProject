using Core.DTO.Response;
using Core.DTO.User;
using Core.DTO.UserDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWTWebAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDTO createUser)
        {
            if (!ModelState.IsValid)
            {
                 return BadRequest(ModelState);
            }

            ResponseGeneralDTO responseGeneralDTO = await _userService.AddAsync(createUser);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ResponseGeneralDTO responseGeneralDTO = await _userService.DeleteByIdAsync(id);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }

        [HttpPost("withrole")]
        public async Task<IActionResult> AddUserWithRole([FromBody] CreateUserWithRoleDTO createUserWithRole)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            responseGeneralDTO = await _userService.AddWithRoleAsync(createUserWithRole);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }
    }
}