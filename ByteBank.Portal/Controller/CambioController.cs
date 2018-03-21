//using ByteBank.Portal.Model;
using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Portal.Filtros;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ByteBank.Service.Cartao;

namespace ByteBank.Portal.Controller
{
    public class CambioController: ControllerBase
    {
        private ICambioService _cambioService;
        private ICartaoService _cartaoService;

        public CambioController(ICambioService cambioService, ICartaoService cartaoService)
        {
            _cambioService = cambioService;
            _cartaoService = cartaoService;
        }

        [ApenasHorarioComercialFilterAttibute]
        public string MXN()
        {
            return View(new {
                Valor = this._cambioService.Calcular("MXN", "BRL", 1)
            });
        }

        [ApenasHorarioComercialFilterAttibute]
        public string USD()
        {         
            return View(new
            {
                Valor = this._cambioService.Calcular("USD", "BRL", 1)
            });
        }

        [ApenasHorarioComercialFilterAttibute]
        public string Calculo(string moedaDestino) => Calculo("BRL", moedaDestino, 1);

        [ApenasHorarioComercialFilterAttibute]
        public string Calculo(string moedaDestino, decimal valor) => Calculo("BRL", moedaDestino, valor);

        [ApenasHorarioComercialFilterAttibute]
        public string Calculo(string moedaOrigem, string moedaDestino, decimal valor)
        {
            var valorFinal = this._cambioService.Calcular(moedaOrigem, moedaDestino, valor);

            var cartaoPromocao = this._cartaoService.ObterCartaoDeCreditoDeDestaque();

            //Quando eu crio um objeto sem parenteses e com chaves é chamado de auto Initialzer
            var modelo = new
            {
                ValorOrigem = valor,
                ValorFinal = valorFinal,
                MoedaDestino = moedaDestino,
                MoedaOrigem = moedaOrigem,
                CartaoPromocao = cartaoPromocao
            };

            return View(modelo);
        }
    }
}

/*
 * O compilador do C#, trata um tipo anônimo como um tipo normal. 
 * Em tempo de compilação, ele traduz a sintaxe que usamos para criar tipo anônimos para uma classe tradicional 
 * e esta possui todas as características - do ponto de vista de Reflection - que uma classe normal.

 
   Correta! Este tipo de recurso da linguagem também é conhecida como syntax-sugar, 
   onde uma sintaxe é usada somente para simplificar construções mais complexas que são criadas automaticamente pelo compilador.

    var tipoQualquer = new {
        Prop1 = 1,
        prop2= "Test"

        };


    tipoQualquer.GetType().GetProperties();

 */
