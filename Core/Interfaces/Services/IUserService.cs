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
        public Task<User?> GetUserAsync(string userName, string password);

        public string? GenerateToken(User user);
    }
}
