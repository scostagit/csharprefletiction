using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Model
{
    public class CalculoCambioModel
    {
        public string MoedaDestino { get; set; }
        public string MoedaOrigem { get; set; }
        public Decimal ValorOrigem { get; set; }
        public Decimal ValorFinal { get; set; }
    }
}
