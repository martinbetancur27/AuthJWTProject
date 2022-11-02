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
        public Task<bool> IsUserInDatabaseByUsernameAsync(string userName);
        public Task<bool> IsUserInDatabaseByIdAsync(int idUser);
        public Task<bool> IsRoleInUserAsync(int idUser, int idRole);
        public Task<bool> IsRoleInDatabaseAsync(int idRole);
        public Task<int?> AddUserAndReturnIdAsync(User user);
        public Task<int?> AddUserWithRoleAndReturnIdUserAsync(User user, UserRole userRole);
        public Task<bool> AddRoleInUserAsync(UserRole userRole);
        public Task<bool> DeleteUserByIdOfDatabaseAsync(User user);
        public Task<bool> DeleteRoleInUserAsync(UserRole userRole);
        public Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole);
        public bool ChangePassword(User user);
    }
}
