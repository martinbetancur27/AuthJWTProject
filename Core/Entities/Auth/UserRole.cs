using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Auth
{
    public class UserRole
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public int IdRole { get; set; }
        public Role Role { get; set; }
    }
}
