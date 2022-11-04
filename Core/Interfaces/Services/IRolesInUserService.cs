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
        public Task<bool> IsRoleAndUserAsync(int idUser, int idRole);
        public Task<int?> AddUserAndPutRoleAndReturnIdUserAsync(User user, UserRole userRole);
        public Task<UserRole?> GetByUserAndRoleAsync(int idUser, int idRole);
        public Task<bool> AddAsync(int idUser, int idRole);
        public Task<bool> DeleteAsync(UserRole userRole);
    }
}
