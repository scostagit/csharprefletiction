using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Service.Cartao
{
    public class CartaoService : ICartaoService
    {
        public string ObterCartaoDeCreditoDeDestaque() => "ByteBank Gold Platinum Extra Premium Special";

        public string ObterCartaoDeDebitoDeDestaque() => "ByteBank Estuando sem taxas de manuntencao";

    }
}
