using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO
{
    public class UserRoleDTO
    {
        [Required]
        public int IdUser { get; set; }
        [Required]
        public int IdRole { get; set; }
    }
}
