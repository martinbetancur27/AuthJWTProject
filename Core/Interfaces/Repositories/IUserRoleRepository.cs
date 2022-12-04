using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        public Task<bool> IsRoleAndUserAsync(int idUser, int idRole);
        public Task<bool> AddAsync(UserRole userRole);
        public Task<bool> DeleteAsync(UserRole userRole);
        public Task<UserRole?> GetByUserAndRoleAsync(int idUser, int idRole);
    }
}
