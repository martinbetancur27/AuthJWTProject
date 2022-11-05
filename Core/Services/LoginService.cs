using Core.DTO.Auth;
using Core.Entities.Auth;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
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
        private readonly IEncryptService _encryptService;

        public LoginService(IUserRepository userRepository, IEncryptService encryptService)
        {
            _userRepository = userRepository;
            _encryptService = encryptService;
        }

        public async Task<User?> LoginAsync(string userName, string password)
        {
            var passwordSha256 = _encryptService.GetSHA256OfString(password);
            
            return await _userRepository.GetByUsernameAndPasswordAsync(userName, passwordSha256);
        }

        public bool ChangePassword(User user, string newPassword)
        {
            user.Password = _encryptService.GetSHA256OfString(newPassword);

            return _userRepository.Update(user);
        }
    }
}
