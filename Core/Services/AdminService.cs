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

        public async Task<bool?> IsUserInDatabaseAsync(string userName)
        {
            return await _userRepository.IsUserInDatabaseAsync(userName);
        }

        public async Task<int?> AddUserAndReturnIdAsync(User editor, int? idRole = null)
        {
            editor.Password = EncryptTool.GetSHA256OfString(editor.Password);
            return await _userRepository.AddUserDatabaseAndReturnIdAsync(editor, idRole);
        }

        public async Task<bool?> DeleteUserAsync(int idUser)
        {
            return await _userRepository.DeleteUserByIdOfDatabaseAsync(idUser);
        }

        public async Task<bool?> DeleteRoleInUserAsync(UserRole userRole)
        {
            return await _userRepository.DeleteRoleInUserAsync(userRole);
        }

        public async Task<UserRole?> GetUserRoleDatabaseAsync(int idUser, int idRole)
        {
            return await _userRepository.GetUserRoleDatabaseAsync(idUser, idRole);
        }

        public async Task<bool?> AddRoleInUserAsync(int idUser, int idRole)
        {
            return await _userRepository.AddRoleInUserAsync(idUser, idRole);
        }
    }
}
