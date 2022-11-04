using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string userName, string password)
        {
            var passwordSha256 = EncryptTool.GetSHA256OfString(password);
            
            return await _userRepository.GetByUsernameAndPasswordAsync(userName, passwordSha256);
        }

        public bool ChangePassword(User user, string newPassword)
        {
            user.Password = EncryptTool.GetSHA256OfString(newPassword);

            return _userRepository.Update(user);
        }
    }
}
