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

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Usuario credenciais) {


            try
            {
                var usuario = this._database.Usuarios.First(user => user.Email.Equals(credenciais.Email));

                if(usuario != null) {
                    if(usuario.Senha.Equals(credenciais.Senha)){
                        Response.StatusCode = 200;
                        return new JsonResult(new{menssage="Login realizado com sucesso", usuario});
                    } else {
                        Response.StatusCode = 401;
                        return new JsonResult(new {menssage="Senha incorreta."});
                    }
                } else {
                    Response.StatusCode = 401;
                    return new JsonResult(new {menssage="Usuario inválido."});
                }
            }
            catch (System.Exception)
            {
                Response.StatusCode = 401;
                return new JsonResult(new {menssage="Usuario inválido."});
            }
        }


        [HttpGet]
        public IActionResult GetUsers(){
            var usuarios = this._database.Usuarios.ToList();
            
            Response.StatusCode = 200;
            return new JsonResult(usuarios);
        }
    }
}