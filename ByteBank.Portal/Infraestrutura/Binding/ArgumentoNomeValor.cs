using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.Binding
{
    public class ArgumentoNomeValor
    {
        public string Nome { get; private set; }
        public string Valor { get; private set; }

        public ArgumentoNomeValor(string nome, string valor)
        {
            this.Nome = nome ?? throw new AggregateException(nameof(nome));
            this.Valor = valor ?? throw new AggregateException(nameof(valor));
        }
    }
}
