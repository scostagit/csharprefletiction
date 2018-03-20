using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.Filtros
{
    public class FilterResult
    {
        public bool PoderContinuar { get; private set; }

        public FilterResult(bool podeContinuar)
        {
            this.PoderContinuar = podeContinuar;
        }
    }
}
