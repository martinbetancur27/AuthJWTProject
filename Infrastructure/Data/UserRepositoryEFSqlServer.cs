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


        public async Task<bool> IsUserInDatabaseByUsernameAsync(string userName)
        {
            return await _databaseContext.Users.Where(x => x.UserName == userName).AnyAsync();
        }


        public async Task<bool> IsUserInDatabaseByIdAsync(int idUser)
        {
            return await _databaseContext.Users.Where(x => x.Id == idUser).AnyAsync();
        }


        public async Task<bool> IsRoleInDatabaseAsync(int idRole)
        {
            return await _databaseContext.Roles.Where(x => x.Id == idRole).AnyAsync();
        }


        public async Task<bool> IsRoleInUserAsync(int idUser, int idRole)
        {
            return await _databaseContext.UserRoles.Where(x => x.IdUser == idUser && x.IdRole == idRole).AnyAsync();
        }


        public async Task<User?> GetUserLoginOfDatabaseAsync(string userName, string password)
        {
            return await _databaseContext.Users.Where(x => x.UserName == userName && x.Password == password).Include(r => r.Roles).FirstOrDefaultAsync();
        }


        public async Task<User?> GetUserByIdOfDatabaseAsync(int idUser)
        {   
            return await _databaseContext.Users.FindAsync(idUser);
        }


        public async Task<int?> AddUserAndReturnIdAsync(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();

            return user.Id;
        }


        public async Task<int?> AddUserWithRoleAndReturnIdUserAsync(User user, UserRole userRole)
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


        public async Task<bool> AddRoleInUserAsync(int idUser, int idRole)
        {
            UserRole userRole = new UserRole
            {
                IdUser = idUser,
                IdRole = idRole
            };

            await _databaseContext.UserRoles.AddAsync(userRole);
            await _databaseContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> ExistRoleInUserAsync(int idUser, int idRole)
        {
            var responseSearch = await _databaseContext.UserRoles.Where(x => x.IdUser == idUser && x.IdRole == idRole).FirstOrDefaultAsync();
                
            if (responseSearch == null)
            {
                return false;
            }

            return true;
        }


        public async Task<bool> DeleteUserByIdOfDatabaseAsync(int idUser)
        {   
            var userFromDb = await GetUserByIdOfDatabaseAsync(idUser);

            if (userFromDb == null)
            {
                return false;
            }

            _databaseContext.Users.Remove(userFromDb);
            await _databaseContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteRoleInUserAsync(UserRole userRole)
        {
            _databaseContext.UserRoles.Remove(userRole);                
            await _databaseContext.SaveChangesAsync();

            return true;
        }


        public async Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole)
        {
            return await _databaseContext.UserRoles.Where(x => x.IdUser == idUser && x.IdRole == idRole).FirstOrDefaultAsync();
        }


        public bool ChangePassword(User user)
        {
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();

            return true;           
        }
    }
}
