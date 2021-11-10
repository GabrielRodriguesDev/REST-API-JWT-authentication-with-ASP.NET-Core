using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST_API_JWT_authentication_with_ASP.NET_Core.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Senha { get; set; }
    }
}