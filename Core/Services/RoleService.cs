using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> IsIdRegisteredAsync(int idRole)
        {
            return await _roleRepository.IsIdRegisteredAsync(idRole);
        }
    }
}