using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<User?> GetByIdAsync(int idUser);
        public Task<User?> FindByUsernameAndPasswordAsync(string userName, string password);
        public Task<bool> IsUsernameRegisteredAsync(string userName);
        public Task<bool> IsIdRegisteredAsync(int idUser);
        public Task<int?> AddAndReturnIdAsync(User user);
        public Task<bool> DeleteByIdAsync(int id);
        public bool Update(User user);
    }
}