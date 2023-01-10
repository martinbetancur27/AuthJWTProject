using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.UserDTO;

namespace Core.DTO.Response
{
    public class ResponseUsersDTO
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "Success";
        public IEnumerable<UsersDTO>? Users { get; set; }
    }
}
