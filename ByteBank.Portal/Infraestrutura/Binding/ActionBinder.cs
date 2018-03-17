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
        public object ObterMethdoInfo(object controller, string path)
        {
            var idxInterrogacao = path.IndexOf('?');
            var existeQuerySring = idxInterrogacao >= 0;

            if (!existeQuerySring)
            {

                var actionName = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var methodInfo = controller.GetType().GetMethod(actionName);
                return methodInfo;
            }
            else
            {
                var nomeControllerComAction = path.Substring(0, idxInterrogacao);
                var nomeAction = nomeControllerComAction.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var queryString = path.Substring(idxInterrogacao + 1);
                var tuplasNomeValor = this.ObterArgumentosNomeValores(queryString);

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

                var methodInfo = controller.GetType().GetMethods(bindFlags);
                return methodInfo;
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
    }
}
