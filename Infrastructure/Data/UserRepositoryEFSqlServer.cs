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

        public async Task<bool> IsUsernameRegisteredAsync(string userName)
        {
            return await _databaseContext.Users.Where(x => x.UserName == userName).AnyAsync();
        }

        public async Task<bool> IsIdRegisteredAsync(int idUser)
        {
            return await _databaseContext.Users.Where(x => x.Id == idUser).AnyAsync();
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string userName, string password)
        {
            return await _databaseContext.Users.Where(x => x.UserName == userName && x.Password == password).Include(r => r.Roles).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(int idUser)
        {   
            return await _databaseContext.Users.FindAsync(idUser);
        }

        public async Task<int?> AddAndReturnIdAsync(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> DeleteAsync(User user)
        {   
            _databaseContext.Users.Remove(user);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public bool Update(User user)
        {
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();

            return true;           
        }

        public async Task<int?> AddAndPutRoleAndReturnIdAsync(User user, UserRole userRole)
        {
            using (var transaction = await _databaseContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _databaseContext.Users.AddAsync(user);
                    await _databaseContext.SaveChangesAsync();

                    userRole.IdUser = user.Id;
                    await _databaseContext.UserRoles.AddAsync(userRole);
                    await _databaseContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return user.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Outer error", ex);
                }
            }
        }
    }
}
