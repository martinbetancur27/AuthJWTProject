using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.DTO.Response
{
    public class ResponseGeneralDTO
    {
        public int Result { get; set; }
        public string Mesagge { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
