using ByteBank.Portal.Infraestrutura.Binding;
using ByteBank.Portal.Infraestrutura.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public class ManipuladorRequisicaoController
    {
        private readonly ActionBinder _actionBinder = new ActionBinder();
        private readonly FilterResolver _filterResolver = new FilterResolver();

        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var partes = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerNome= partes[0];
            var action = partes[1];
            var controllerNomeCompleto = $"ByteBank.Portal.Controller.{controllerNome}Controller";

            //Activator: voce consegue criar object pelo nome.
            var controllerWrapper = Activator.CreateInstance("ByteBank.Portal", controllerNomeCompleto, new object[0]);

            /*
             * O método Activator::CreateInstance pode ser usado para criar objetos em um AppDomain diferente de nosso código em execução e a 
             * manipulação de objetos criados em outros AppDomains é feita através de um ObjectHandle. 
             * 
             * Correta! Com o Activator, os objetos criados são encapsulados em um ObjectHandle.
             */
            var controller = controllerWrapper.Unwrap();

            // var methodInfo = controller.GetType().GetMethod(action);
            var actionBindInfo = this._actionBinder.ObterActionBindInfo(controller, path);


            var filterResult = this._filterResolver.VerificarFiltros(actionBindInfo);


            if (filterResult.PoderContinuar)
            {
                //para invocar um metodo via reflection eu preciso informar a instande em que ele esta, e passar a lista de parametros
                var resultadoAction = (string)actionBindInfo.Invoke(controller);

                var buffer = Encoding.UTF8.GetBytes(resultadoAction);

                resposta.StatusCode = 200;
                resposta.ContentType = "text/html; charset=utf-8";
                resposta.ContentLength64 = resultadoAction.Length;               
                resposta.OutputStream.Write(buffer, 0, buffer.Length);
                resposta.OutputStream.Close();

            }
            else
            {
                resposta.StatusCode = 307;
                resposta.RedirectLocation = "/Erro/Inesperado";
                resposta.OutputStream.Close();
            }          
        }
    }
}
