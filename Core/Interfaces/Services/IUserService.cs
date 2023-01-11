using Core.DTO.Parameters;
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
    public interface IUserService
    {
        public Task<ResponseGeneralDTO> AddAsync(CreateUserDTO createUser);
        public Task<ResponseUsersDTO> GetListAsync(UserParametersDTO userParameters);
        public Task<int?> CountAsync();
        public Task<ResponseGeneralDTO> DeleteByIdAsync(int id);
        public Task<ResponseGeneralDTO> AddWithRoleAsync(CreateUserWithRoleDTO createUserWithRole);
        public Task<User?> FindByUsernameAndPasswordAsync(string userName, string password);
        public bool Update(User user);
    }
}