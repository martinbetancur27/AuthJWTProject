using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Tools;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User?> GetByIdAsync(int idUser)
        {
            return await _userRepository.GetByIdAsync(idUser);
        }


        public async Task<bool> IsUsernameRegisteredAsync(string userName)
        {
            return await _userRepository.IsUsernameRegisteredAsync(userName);
        }


        public async Task<bool> IsIdRegisteredAsync(int idUser)
        {
            return await _userRepository.IsIdRegisteredAsync(idUser);
        }


        public async Task<int?> AddAndReturnIdAsync(User user)
        {
            user.Password = EncryptTool.GetSHA256OfString(user.Password);

            return await _userRepository.AddAndReturnIdAsync(user);
        }


        public async Task<bool> DeleteByIdAsync(int idUser)
        {
            User user = new User
            {
                Id = idUser
            };

            return await _userRepository.DeleteAsync(user);
        }


        public bool Update(User user)
        {
            return _userRepository.Update(user);
        }
    }
}
