using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Response
{
    public class ResponseLoginDTO
    {
        public int Result { get; set; }
        public string Mesagge { get; set; }
        public int ExpireInMinutes { get; set; }
        public string Token { get; set; }
    }
}
