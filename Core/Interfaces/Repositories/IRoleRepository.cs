using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task<bool> IsIdRegisteredAsync(int idRole);
    }
}
