using System.Linq;
using Microsoft.AspNetCore.Mvc;
using REST_API_JWT_authentication_with_ASP.NET_Core.Models;
using REST_API_JWT_authentication_with_ASP.NET_Core.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

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


                            //Chave de segurança
                        string chaveDeSeguranca = "GabrielSilvaRodriguesMota";

                        var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveDeSeguranca));
                        var credenciaisDeAcesso = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

                        var JWT = new JwtSecurityToken(
                            issuer: "Gabriel Silva Rodrigues Mota", //Quem está fornecendo o JWT para o usuario.
                            expires: DateTime.UtcNow.AddHours(2), //Token válido por 1 hora
                            audience: "usuario_comum",
                            signingCredentials: credenciaisDeAcesso
                        ); 



                        Response.StatusCode = 200;
                        return new JsonResult(new JwtSecurityTokenHandler().WriteToken(JWT));
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