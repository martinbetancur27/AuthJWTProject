using Core.DTO.UserDTO;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetByUsernameAndPasswordAsync(string userName, string password);
        public Task<User?> GetByIdAsync(int idUser);
        public IEnumerable<UsersDTO>? GetAsync();
        public Task<bool> IsUsernameRegisteredAsync(string userName);
        public Task<bool> IsIdRegisteredAsync(int idUser);
        public Task<int?> AddAndReturnIdAsync(User user);
        public Task<int?> AddAndPutRoleAndReturnIdAsync(User user, UserRole userRole);
        public Task<bool> DeleteAsync(User user);
        public bool Update(User user);
    }
}
