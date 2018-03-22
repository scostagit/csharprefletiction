using ByteBank.Portal.Infraestrutura.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public class ControllerResolver
    {
        private readonly IContainer _container;
       

        public ControllerResolver(IContainer container)
        {
            this._container = container;
        }

        //nomeController = ByteBank.Portal.Controller.CambioController
        public object ObterController(string nomeController)
        {
            var tipoController = Type.GetType(nomeController);
            var instanciaController = this._container.Recuperar(tipoController);
            return instanciaController;
        }
    }
}

