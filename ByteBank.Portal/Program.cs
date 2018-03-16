using ByteBank.Portal.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal
{
    class Program
    {
        static void Main(string[] args)
        {
            //Apenas um prefixo para nosso aplicação. Nosso servidor vai ouvir essa url.
            var prefixos = new string[] { "http://localhost:8071/" };
            var webApplication = new WebApplicationAlura(prefixos);
            webApplication.Iniciar();
            Console.Write("Servidor Ativo");
        }
    }
}
