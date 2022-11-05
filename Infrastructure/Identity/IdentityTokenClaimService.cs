using Core.Constants;
using Core.Entities.Auth;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityTokenClaimService : ITokenService
    {
        private IConfiguration _config;

        public IdentityTokenClaimService(IConfiguration config)
        {
            _config = config;
        }


        public string? GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Authentication, user.Id.ToString()),
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
              expires: DateTime.Now.AddMinutes(TokenConstants.ExpireInMinutes),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
