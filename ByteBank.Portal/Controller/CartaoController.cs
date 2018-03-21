using ByteBank.Service;
using ByteBank.Service.Cartao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Controller
{
    public class CartaoController : ControllerBase
    {
        private ICartaoService _cartaoService;

        public CartaoController(ICartaoService cartaoService)
        {
            _cartaoService = cartaoService;
        }

        public string Debito() => 
            View(new {CartaoNome = this._cartaoService.ObterCartaoDeDebitoDeDestaque() });

        public string Cretido() => 
            View(new { CartaoNome = this._cartaoService.ObterCartaoDeCreditoDeDestaque() });
    }
}
