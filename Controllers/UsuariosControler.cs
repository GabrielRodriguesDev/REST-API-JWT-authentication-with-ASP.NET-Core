using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST_API_JWT_authentication_with_ASP.NET_Core.Models;
using REST_API_JWT_authentication_with_ASP.NET_Core.Data;

namespace REST_API_JWT_authentication_with_ASP.NET_Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosControler : ControllerBase
    {
        private ApplicationDbContext _database;
        public UsuariosControler(ApplicationDbContext database)
        {
            this._database = database;
        }
        //UsuariosControler/registro
        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario usuario){
            this._database.Add(usuario);
            this._database.SaveChanges();
            return Ok(new{mensage= "Usuario cadastrado com sucesso", usuario });
        }
    }
}