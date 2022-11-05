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
    public class RolesInUserService : IRolesInUserService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEncryptService _encryptService;

        public RolesInUserService(IUserRoleRepository userRoleRepository, IEncryptService encryptService)
        {
            _userRoleRepository = userRoleRepository;
            _encryptService = encryptService;
        }


        public async Task<bool> AddAsync(int idUser, int idRole)
        {
            UserRole userRole = new UserRole
            {
                IdUser = idUser,
                IdRole = idRole
            };
            return await _userRoleRepository.AddAsync(userRole);
        }


        public async Task<int?> AddUserAndPutRoleAndReturnIdUserAsync(User user, UserRole userRole)
        {
            user.Password = _encryptService.GetSHA256OfString(user.Password);

            return await _userRoleRepository.AddUserAndPutRoleAndReturnIdUserAsync(user, userRole);
        }


        public async Task<bool> IsRoleAndUserAsync(int idUser, int idRole)
        {
            return await _userRoleRepository.IsRoleAndUserAsync(idUser, idRole);
        }


        public async Task<bool> DeleteAsync(UserRole userRole)
        {
            return await _userRoleRepository.DeleteAsync(userRole);
        }


        public async Task<UserRole?> GetByUserAndRoleAsync(int idUser, int idRole)
        {
            return await _userRoleRepository.GetByUserAndRoleAsync(idUser, idRole);
        }
    }
}
