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
    public class UserRepositoryEFSqlServer : IUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UserRepositoryEFSqlServer(ApplicationDbContext db)
        {
            _databaseContext = db;
        }

        public async Task<User?> GetUserOfDatabaseAsync(string userName, string password)
        {
            try
            {
                return await _databaseContext.Users.Where(x => x.UserName == userName && x.Password == password).Include(r => r.Roles).FirstOrDefaultAsync();
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
