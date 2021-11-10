using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST_API_JWT_authentication_with_ASP.NET_Core.Models
{
    public class Produto

    {


        public Produto()
        {
            this.Id = Guid.NewGuid(); // Gerando um ID automatico ao criar um objeto da classe, assim ao mandar um POST referente a essa classe, não é necessário mandar o ID na requisição o próprio sistema cuida de gerar
        }
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
    }
}
