using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptService _encryptService;

        public UserService(IUserRepository userRepository, IEncryptService encryptService)
        {
            _userRepository = userRepository;
            _encryptService = encryptService;
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


        public async Task<User?> FindByUsernameAndPasswordAsync(string userName, string password)
        {
            var passwordSha256 = _encryptService.GetSHA256OfString(password);
            
            return await _userRepository.GetByUsernameAndPasswordAsync(userName, passwordSha256);
        }


        public async Task<int?> AddAndReturnIdAsync(User user)
        {
            user.Password = _encryptService.GetSHA256OfString(user.Password);

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