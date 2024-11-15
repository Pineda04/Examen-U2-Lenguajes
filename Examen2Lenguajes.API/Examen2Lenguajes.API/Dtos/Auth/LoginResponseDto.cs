using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Lenguajes.API.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }

    }
}