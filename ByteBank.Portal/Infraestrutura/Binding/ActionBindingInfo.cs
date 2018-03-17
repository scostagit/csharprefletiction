using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.Binding
{
    public class ActionBindingInfo
    {
        public MethodInfo MethodInfo { get; private set; }
        public IReadOnlyCollection<ArgumentoNomeValor> TuplasArgumentosNomeValor { get; private set; }

        public ActionBindingInfo(MethodInfo methodInfo, IEnumerable<ArgumentoNomeValor> tuplasArgumentosNomeValor)
        {
            this.MethodInfo = methodInfo ?? throw new ArgumentException(nameof(methodInfo));
            if (tuplasArgumentosNomeValor == null)
                throw new ArgumentException(nameof(tuplasArgumentosNomeValor));
           
            this.TuplasArgumentosNomeValor = new ReadOnlyCollection<ArgumentoNomeValor>(tuplasArgumentosNomeValor.ToList());
        }

        /*
         * Nesta aula, por que foi necessário tomarmos um cuidado especial com a correta ordenação dos argumentos?
         * 
         * As mesmas restrições que se aplicam a linguagem, se aplicam ao código chamado por meio de reflection.
         * Deste modo, a ordem dos argumentos e seus tipos devem ser respeitadas.
         * 
         * As restrições de invocação de métodos são as mesmas para o compilador e para o código dinâmico.
         */

        public object Invoke(object controller)
        {
            var countParametros = this.TuplasArgumentosNomeValor.Count;
            var possuiArgumentos = countParametros > 0;

            if (!possuiArgumentos)            
               return  this.MethodInfo.Invoke(controller, new object[0]);

            var parametrosMethodInfo = this.MethodInfo.GetParameters();
            var parametrosInvoke = new object[countParametros];

            //Varemos todos os nossos parametros
            for (int i = 0; i < countParametros; i++)
            {
                var parametro = parametrosMethodInfo[i];
                //acessamos o nome do parametro
                var parametroNome = parametro.Name;
                //pegamos o item de paramtros da requisição equivalente ao nome do paramtro.
                var argumento = this.TuplasArgumentosNomeValor.Single(tupla => tupla.Nome.Equals(parametroNome));
                //Guardamos de forma ordenada.
                parametrosInvoke[i] =  Convert.ChangeType( argumento.Valor, parametro.ParameterType);
            }


           return  this.MethodInfo.Invoke(controller, parametrosInvoke);
        }
    }
}
