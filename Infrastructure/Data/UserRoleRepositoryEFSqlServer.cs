using Core.Entities.Auth;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRoleRepositoryEFSqlServer : IUserRoleRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UserRoleRepositoryEFSqlServer(ApplicationDbContext db)
        {
            _databaseContext = db;
        }

        public async Task<UserRole?> GetByUserAndRoleAsync(int idUser, int idRole)
        {
            return await _databaseContext.UserRoles.Where(x => x.IdUser == idUser && x.IdRole == idRole).FirstOrDefaultAsync();
        }

        public async Task<bool> IsRoleAndUserAsync(int idUser, int idRole)
        {
            return await _databaseContext.UserRoles.Where(x => x.IdUser == idUser && x.IdRole == idRole).AnyAsync();
        }

        public async Task<bool> AddAsync(UserRole userRole)
        {
            await _databaseContext.UserRoles.AddAsync(userRole);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(UserRole userRole)
        {
            _databaseContext.UserRoles.Remove(userRole);
            await _databaseContext.SaveChangesAsync();

            return true;
        }
    }
}
