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
        public Task<User?> GetUserLoginOfDatabaseAsync(string userName, string password);
        public Task<User?> GetUserByIdOfDatabaseAsync(int idUser);
        public Task<bool> IsUserInDatabaseAsync(string userName);
        public Task<int?> AddUserDatabaseAndReturnIdAsync(User user, int? idRole = null);
        public Task<bool> AddRoleInUserAsync(int idUser, int idRole);
        public Task<bool> DeleteUserByIdOfDatabaseAsync(int idUser);
        public Task<bool> DeleteRoleInUserAsync(UserRole userRole);
        public Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole);
        public bool ChangePassword(User user);
    }
}
