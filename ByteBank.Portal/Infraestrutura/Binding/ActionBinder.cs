using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.Binding
{

    //Classe especializada  por fazer a ligação entre a nossa classe MethodInfo e nossa URL.
    public class ActionBinder
    {
        public ActionBindingInfo ObterActionBindInfo(object controller, string path)
        {
            var idxInterrogacao = path.IndexOf('?');
            var existeQuerySring = idxInterrogacao >= 0;           

            if (!existeQuerySring)
            {

                var actionName = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var methodInfo = controller.GetType().GetMethod(actionName);
                return new ActionBindingInfo(methodInfo, Enumerable.Empty<ArgumentoNomeValor>());   
            }
            else
            {
                var nomeControllerComAction = path.Substring(0, idxInterrogacao);
                var nomeAction = nomeControllerComAction.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var queryString = path.Substring(idxInterrogacao + 1);
                var tuplasNomeValor = this.ObterArgumentosNomeValores(queryString);

                var argumentos = tuplasNomeValor.Select(tupla => tupla.Nome).ToArray();

                var methodInfo = ObterMethodInfoAPartirDeNomeEArgumentos(nomeAction, argumentos, controller);
                return new ActionBindingInfo(methodInfo, tuplasNomeValor);
            }
           
        }

        private IEnumerable<ArgumentoNomeValor> ObterArgumentosNomeValores(string queryString)
        {
            var TuplasNomeVAlor = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var tupla in TuplasNomeVAlor)
            {
                var partesTulpas = tupla.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                yield return new ArgumentoNomeValor(partesTulpas[0], partesTulpas[1]);
            }
        }


        private MethodInfo ObterMethodInfoAPartirDeNomeEArgumentos(string nomeAction, string[] argumentos, object controller)
        {

            var arumentosCount = argumentos.Length;

            /*
             * Nesta aula usamos os operadores bitwise (ou operadores bit-a-bit) no momento de lidar com os nossos filtros de alguns métodos do Reflection!
                Para saber mais sobre os operadores bit-a-bit, você pode verificar a documentação da Microsoft disponível aqui!
                Aproveite também e veja a documentação do enumerador BindingFlags aqui!
                Ficou com dúvida? Recorra ao nosso fórum, não perca tempo! :)
                https://msdn.microsoft.com/pt-br/library/17zwb64t.aspx
                https://msdn.microsoft.com/pt-br/library/system.reflection.bindingflags.aspx


            GetMethod x GetMethods
            O método Type::GetMethods() retorna todos os métodos de um Tipo enquanto que o método Type::GetMethod(string) espera o nome de um 
            método com somente uma sobrecarga, caso contrário, uma exceção de ambiguidade é lançada.

            OBS:Os métodos privados não são retornados! os métodos públicos e protegidos da classe mãe também são retornados.
             */
            //Varivel para filtrar informações no GetMethods
            var bindFlags =
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly; //vai trazer so mehtod declarados na minha classe.

            var metodos = controller.GetType().GetMethods(bindFlags);
            var sobrecargas = metodos.Where(metodo => metodo.Name.Equals(nomeAction));

            foreach (var sobrecarga in sobrecargas)
            {
                var paramtros = sobrecarga.GetParameters();

                if (paramtros.Length != arumentosCount)
                    continue;

                //All: Vai executar uma condição para todos os item do paramtros
                var match = paramtros.All(parametro => argumentos.Contains(parametro.Name));

                if (match) return sobrecarga;                
            }

            throw new ArgumentException($"A sobrecarga do metodo {nomeAction} nao foi encontrata");
        }
    }
}
