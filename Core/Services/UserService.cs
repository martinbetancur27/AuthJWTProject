using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Entities.Auth;
using Core.DTO.Response;
using Core.DTO.User;
using Core.DTO.UserDTO;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptService _encryptService;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IRoleService roleService, IEncryptService encryptService)
        {
            _userRepository = userRepository;
            _encryptService = encryptService;
            _roleService = roleService;
        }

        public async Task<ResponseGeneralDTO> AddAsync(CreateUserDTO createUser)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();
            
            var isUserInDatabase = await _userRepository.IsUsernameRegisteredAsync(createUser.UserName);

            if (isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 403;
                responseGeneralDTO.Message = "Username already exists";

                return responseGeneralDTO;
            }

            User user = new User
            {
                Name = createUser.Name,
                UserName = createUser.UserName,
                Password = createUser.Password
            };

            user.Password = _encryptService.GetSHA256OfString(user.Password);

            await _userRepository.AddAndReturnIdAsync(user);

            responseGeneralDTO.StatusCode = 201;
            responseGeneralDTO.Message = "The user has been created";

            return responseGeneralDTO;
        }

        public async Task<ResponseGeneralDTO> DeleteByIdAsync(int id)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var isUserInDatabase = await _userRepository.IsIdRegisteredAsync(id);

            if (!isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "User Id does not exist";

                return responseGeneralDTO;
            }

            User user = new User
            {
                Id = id
            };

            await _userRepository.DeleteAsync(user);

            responseGeneralDTO.StatusCode = 200;
            responseGeneralDTO.Message = "The user has been deleted";

            return responseGeneralDTO;
        }

        public async Task<ResponseGeneralDTO> AddWithRoleAsync(CreateUserWithRoleDTO createUserWithRole)
        {
            ResponseGeneralDTO responseGeneralDTO = new ResponseGeneralDTO();

            var isRoleInDatabase = await _roleService.IsIdRegisteredAsync(createUserWithRole.idRole);

            if (!isRoleInDatabase)
            {
                responseGeneralDTO.StatusCode = 404;
                responseGeneralDTO.Message = "The id role does not exist";

                return responseGeneralDTO;
            }

            var isUserInDatabase = await _userRepository.IsUsernameRegisteredAsync(createUserWithRole.UserName);

            if (isUserInDatabase)
            {
                responseGeneralDTO.StatusCode = 403;
                responseGeneralDTO.Message = "Username already exists";

                return responseGeneralDTO;
            }

            UserRole userRole = new UserRole
            {
                IdRole = createUserWithRole.idRole
            };

            User user = new User
            {
                Name = createUserWithRole.Name,
                UserName = createUserWithRole.UserName,
                Password = _encryptService.GetSHA256OfString(createUserWithRole.Password)
            };

            await _userRepository.AddAndPutRoleAndReturnIdAsync(user, userRole);

            responseGeneralDTO.StatusCode = 201;
            responseGeneralDTO.Message = "The user has been created";

            return responseGeneralDTO;
        }

        public async Task<User?> FindByUsernameAndPasswordAsync(string userName, string password)
        {
            var passwordSha256 = _encryptService.GetSHA256OfString(password);
            
            return await _userRepository.GetByUsernameAndPasswordAsync(userName, passwordSha256);
        }
        
        public bool Update(User user)
        {
            return _userRepository.Update(user);
        }
    }
}