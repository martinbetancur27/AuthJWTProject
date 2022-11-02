using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IAdminService
    {
        public Task<bool> IsUserInDatabaseByUsernameAsync(string userName);
        public Task<bool> IsUserInDatabaseByIdAsync(int idUser);
        public Task<bool> IsRoleInDatabaseAsync(int idRole);
        public Task<bool> IsRoleInUserAsync(int idUser, int idRole);
        public Task<int?> AddUserAndReturnIdAsync(User user);
        public Task<int?> AddUserWithRoleAndReturnIdUserAsync(User user, UserRole userRole);
        public Task<bool> DeleteUserAsync(int idUser);
        public Task<bool> DeleteRoleInUserAsync(UserRole userRole);
        public Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole);
        public Task<bool> AddRoleInUserAsync(int idUser, int idRole);
    }
}
