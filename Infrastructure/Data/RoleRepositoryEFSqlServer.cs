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
    public class RoleRepositoryEFSqlServer : IRoleRepository
    {

        private readonly ApplicationDbContext _databaseContext;

        public RoleRepositoryEFSqlServer(ApplicationDbContext db)
        {
            _databaseContext = db;
        }

        public async Task<bool> IsIdRegisteredAsync(int idRole)
        {
            return await _databaseContext.Roles.Where(x => x.Id == idRole).AnyAsync();
        }
    }
}
