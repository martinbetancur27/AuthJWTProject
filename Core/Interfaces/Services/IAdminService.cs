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
        public Task<bool?> IsUserInDatabaseAsync(string userName);
        public Task<int?> AddUserAndReturnIdAsync(User editor, int? idRole = null);
        public Task<bool?> DeleteUserAsync(int idUser);
        public Task<bool?> DeleteRoleInUserAsync(int idUser, int idRole);
        public Task<bool?> AddRoleInUserAsync(int idUser, int idRole);
        public Task<bool?> ExistRoleInUserAsync(int idUser, int idRole);
    }
}
