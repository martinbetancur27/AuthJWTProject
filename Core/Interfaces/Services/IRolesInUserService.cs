using Core.DTO.Response;
using Core.DTO.UserDTO;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IRolesInUserService
    {
        public Task<ResponseGeneralDTO> AddAsync(UserRoleDTO newRoleInUser);
        public Task<ResponseGeneralDTO> DeleteAsync(UserRoleDTO deleteRoleInUser);
    }
}
