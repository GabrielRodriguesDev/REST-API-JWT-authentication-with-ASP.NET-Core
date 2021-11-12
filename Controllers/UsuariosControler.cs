using System.Linq;
using Microsoft.AspNetCore.Mvc;
using REST_API_JWT_authentication_with_ASP.NET_Core.Models;
using REST_API_JWT_authentication_with_ASP.NET_Core.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

                        //Criando a Claims do token
                        var claims = new List<Claim>();
                        claims.Add(new Claim("id" ,usuario.Id.ToString()));
                        claims.Add(new Claim("email",usuario.Email));
                        claims.Add(new Claim(ClaimTypes.Role,"Admin")); // Criando a role que determina se é admin ou não (geralmente)
                        

                        var JWT = new JwtSecurityToken(
                            issuer: "Gabriel Silva Rodrigues Mota", //Quem está fornecendo o JWT para o usuario.
                            expires: DateTime.UtcNow.AddHours(2), //Token válido por 1 hora
                            audience: "usuario_comum",
                            signingCredentials: credenciaisDeAcesso,
                            claims: claims //Passando a Claim na criação do JWT
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


        [HttpGet("VisualizaClaims")]
        [Authorize]
        public IActionResult GetUserClaims(){
            //Recuperando a claim do user que acessou a rota
            var claim = HttpContext.User.Claims.First(claim => claim.Type.ToString().Equals("id", StringComparison.CurrentCultureIgnoreCase));
            return Ok(claim.Value);
        }
    }
}