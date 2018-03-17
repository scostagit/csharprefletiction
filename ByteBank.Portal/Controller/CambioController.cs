using ByteBank.Service;
using ByteBank.Service.Cambio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Controller
{
    public class CambioController
    {
        private ICambioService _cambioService;
        public CambioController()
        {
            _cambioService = new CambioTestService();
        }
        public string MXN()
        {
            var valorFinal = this._cambioService.Calcular("MXN", "BRL", 1);

            //Pego o meu recurso no assemblu
            var nomeCompletoResource = "ByteBank.Portal.View.Cambio.MXN.html";
            var assembly = Assembly.GetExecutingAssembly();

            //Ele me retorna o stream
            var streamRecurso = assembly.GetManifestResourceStream(nomeCompletoResource);

            //Eu tranformo esse stream em string.
            var streamLeitura = new StreamReader(streamRecurso);

            //Agora eu tenho o valor em string, agora posso manipular esse texto injetnado o valor da variavel valorFinal.
            var textoPagina = streamLeitura.ReadToEnd();

            var textoResultado = textoPagina.Replace("VALOR_EM_REAIS", valorFinal.ToString());

            return textoResultado;
        }


        public string USD()
        {
            var valorFinal = this._cambioService.Calcular("USD", "BRL", 1);

            //Pego o meu recurso no assemblu
            var nomeCompletoResource = "ByteBank.Portal.View.Cambio.USD.html";
            var assembly = Assembly.GetExecutingAssembly();

            //Ele me retorna o stream
            var streamRecurso = assembly.GetManifestResourceStream(nomeCompletoResource);

            //Eu tranformo esse stream em string.
            var streamLeitura = new StreamReader(streamRecurso);

            //Agora eu tenho o valor em string, agora posso manipular esse texto injetnado o valor da variavel valorFinal.
            var textoPagina = streamLeitura.ReadToEnd();

            var textoResultado = textoPagina.Replace("VALOR_EM_REAIS", valorFinal.ToString());

            return textoResultado;
        }
    }
}
