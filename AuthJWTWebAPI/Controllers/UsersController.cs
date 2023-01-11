using Core.DTO.Parameters;
using Core.DTO.Response;
using Core.DTO.UserDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpGet()]
        public async Task<IActionResult> GetUsers([FromQuery] UserParametersDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseUsersDTO responseUserDTO = await _userService.GetListAsync(parameters);

            var responsePaginationDTO = new ResponsePaginationDTO
            (
                await _userService.CountAsync() ?? 0, parameters.Page, parameters.PageSize
            );

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(responsePaginationDTO));

            return new ObjectResult(responseUserDTO) { StatusCode = responseUserDTO.StatusCode };
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseGeneralDTO responseGeneralDTO = await _userService.AddWithRoleAsync(createUserWithRole);

            return new ObjectResult(responseGeneralDTO) { StatusCode = responseGeneralDTO.StatusCode };
        }
    }
}