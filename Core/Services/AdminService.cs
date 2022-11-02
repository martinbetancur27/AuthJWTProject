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
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<bool> IsUserInDatabaseByUsernameAsync(string userName)
        {
            return await _userRepository.IsUserInDatabaseByUsernameAsync(userName);
        }


        public async Task<bool> IsUserInDatabaseByIdAsync(int idUser)
        {
            return await _userRepository.IsUserInDatabaseByIdAsync(idUser);
        }


        public async Task<bool> IsRoleInDatabaseAsync(int idRole)
        {
            return await _userRepository.IsRoleInDatabaseAsync(idRole);
        }


        public async Task<bool> IsRoleInUserAsync(int idUser, int idRole)
        {
            return await _userRepository.IsRoleInUserAsync(idUser, idRole);
        }


        public async Task<int?> AddUserAndReturnIdAsync(User user)
        {
            user.Password = EncryptTool.GetSHA256OfString(user.Password);

            return await _userRepository.AddUserAndReturnIdAsync(user);             
        }


        public async Task<int?> AddUserWithRoleAndReturnIdUserAsync(User user, UserRole userRole)
        {
            user.Password = EncryptTool.GetSHA256OfString(user.Password);

            return await _userRepository.AddUserWithRoleAndReturnIdUserAsync(user, userRole);
        }


        public async Task<bool> DeleteUserAsync(int idUser)
        {
            return await _userRepository.DeleteUserByIdOfDatabaseAsync(idUser);
        }


        public async Task<bool> DeleteRoleInUserAsync(UserRole userRole)
        {
            return await _userRepository.DeleteRoleInUserAsync(userRole);
        }

        public async Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole)
        {
            return await _userRepository.GetUserRoleDatabaseAsync(idUser, idRole);
        }


        public async Task<bool> AddRoleInUserAsync(int idUser, int idRole)
        {
            return await _userRepository.AddRoleInUserAsync(idUser, idRole);
        }
    }
}
