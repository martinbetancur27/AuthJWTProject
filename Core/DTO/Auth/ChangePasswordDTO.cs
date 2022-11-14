using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Auth
{
    public class ChangePasswordDTO
    {
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [MaxLength(256)]
        public string CurrentPassword { get; set; }
        [Required]
        [MaxLength(256)]
        public string NewPassword { get; set; }
        [Required]
        [MaxLength(256)]
        public string NewPasswordAgain { get; set; }
    }
}
