using Core.DTO.Auth;
using Core.DTO.Response;
using Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ILoginService
    {
        public Task<ResponseLoginDTO> LoginAsync(UserLoginDTO userLogin);
        public Task<ResponseGeneralDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
    }
}