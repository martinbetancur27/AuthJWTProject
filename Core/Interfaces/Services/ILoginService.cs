using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ILoginService
    {
        public Task<User> LoginAsync(string username, string password);
        public bool ChangePassword(User user, string newPassword);
    }
}
