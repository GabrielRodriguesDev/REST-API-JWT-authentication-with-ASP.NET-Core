using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REST_API_JWT_authentication_with_ASP.NET_Core.Models;

namespace REST_API_JWT_authentication_with_ASP.NET_Core.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base (options)
        {
        }

        public DbSet<Produto> Produtos {get;set;}
        public DbSet<Usuario> Usuarios {get;set;}
    }
}