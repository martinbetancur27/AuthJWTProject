using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Core.Tools;
using Core.Constants;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User?> GetUserAsync(string userName, string password)
        {
            var passwordSha256 = EncryptTool.GetSHA256OfString(password);
            return await _userRepository.GetUserLoginOfDatabaseAsync(userName, passwordSha256);
        }


        public async Task<User?> GetUserByIdOfDatabaseAsync(int idUser)
        {
            return await _userRepository.GetUserByIdOfDatabaseAsync(idUser);
        }


        public bool? ChangePassword(User user)
        {
            return _userRepository.ChangePassword(user);
        }
    }
}
