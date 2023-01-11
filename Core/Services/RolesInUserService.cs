using Core.DTO.Response;
using Core.DTO.UserDTO;
using Core.Entities.Auth;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class RolesInUserService : IRolesInUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RolesInUserService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ResponseGeneralDTO> AddAsync(UserRoleDTO newRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();
            
            var isUserInDatabase = await _userRepository.IsIdRegisteredAsync(newRoleInUser.IdUser);

            if (!isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "User Id does not exist";

                return responseGeneralDTO;
            }

            var isRoleInDatabase = await _roleRepository.IsIdRegisteredAsync(newRoleInUser.IdRole);

            if (!isRoleInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "Role Id does not exist";

                return responseGeneralDTO;
            }

            var isRoleInUserFromDb = await _userRoleRepository.IsRoleAndUserAsync(newRoleInUser.IdUser, newRoleInUser.IdRole);

            if (isRoleInUserFromDb)
            {
                responseGeneralDTO.StatusCode = 400;
                responseGeneralDTO.Message = "User already has that role";

                return responseGeneralDTO;
            }

            UserRole userRole = new UserRole
            {
                IdUser = newRoleInUser.IdUser,
                IdRole = newRoleInUser.IdRole
            };

            await _userRoleRepository.AddAsync(userRole);

            responseGeneralDTO.StatusCode = 201;
            responseGeneralDTO.Message = "Role saved in the user";

            return responseGeneralDTO;
        }

        public async Task<ResponseGeneralDTO> DeleteAsync(UserRoleDTO deleteRoleInUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var isUserInDatabase = await _userRepository.IsIdRegisteredAsync(deleteRoleInUser.IdUser);

            if (!isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "User Id does not exist";

                return responseGeneralDTO;
            }

            var isRoleInDatabase = await _roleRepository.IsIdRegisteredAsync(deleteRoleInUser.IdRole);

            if (!isRoleInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "Role Id does not exist";

                return responseGeneralDTO;
            }

            var roleInUserFromDb = await _userRoleRepository.GetByUserAndRoleAsync(deleteRoleInUser.IdUser, deleteRoleInUser.IdRole);

            if (roleInUserFromDb == null)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "User does not have that role";

                return responseGeneralDTO;
            }

            await _userRoleRepository.DeleteAsync(roleInUserFromDb);

            responseGeneralDTO.StatusCode = 200;
            responseGeneralDTO.Message = "Role removed in the user";

            return responseGeneralDTO;            
        }
    }
}