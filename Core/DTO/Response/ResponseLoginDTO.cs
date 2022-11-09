using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Response
{
    public class ResponseLoginDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int ExpireInMinutes { get; set; }
        public string Token { get; set; }
    }
}
