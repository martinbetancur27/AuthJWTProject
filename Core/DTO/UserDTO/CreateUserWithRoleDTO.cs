﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO
{
    public class CreateUserWithRoleDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        public int idRole { get; set; }
    }
}
