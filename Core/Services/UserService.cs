using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Core.Tools;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IConfiguration _config;

        public UserService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }


        public async Task<User?> GetUserAsync(string userName, string password)
        {
            var passwordSha256 = EncryptTool.GetSHA256OfString(password);
            return await _userRepository.GetUserLoginOfDatabaseAsync(userName, passwordSha256);
        }

        public string? GenerateToken(User user)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var identity = new ClaimsIdentity(new[] 
            {
            
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            });
            
            foreach (var userRole in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, userRole.Name));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              identity.Claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
