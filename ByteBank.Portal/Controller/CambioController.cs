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
    public class CambioController: ControllerBase
    {
        private ICambioService _cambioService;
        public CambioController()
        {
            _cambioService = new CambioTestService();
        }
        public string MXN()
        {
            var valorFinal = this._cambioService.Calcular("MXN", "BRL", 1);

            var textoPagina = View();
            var textoResultado = textoPagina.Replace("VALOR_EM_REAIS", valorFinal.ToString());

            return textoResultado;
        }


        public string USD()
        {
            var valorFinal = this._cambioService.Calcular("USD", "BRL", 1);

            var textoPagina = View();
            var textoResultado = textoPagina.Replace("VALOR_EM_REAIS", valorFinal.ToString());

            return textoResultado;
        }

        public string Calculo(string moedaOrigem, string moedaDestino, decimal valor)
        {
            var valorFinal =  this._cambioService.Calcular(moedaOrigem, moedaDestino, valor);

            var textoPagina = View();
            var textoResultado = textoPagina.Replace("VALOR_MOEDA_ORIGEM", valor.ToString())
                                            .Replace("MOEDA_ORIGEM", moedaOrigem)
                                            .Replace("VALOR_MOEDA_DESTINO", valorFinal.ToString())
                                            .Replace("MOEDA_DESTINO", moedaDestino);

            return textoResultado;
        }

        public string Calculo(string moedaDestino, decimal valor) => Calculo("BRL", moedaDestino, valor);
    }
}
