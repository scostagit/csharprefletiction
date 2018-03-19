using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ByteBank.Portal.Controller
{
    public abstract class ControllerBase
    {

        /*
         * CallerMemberName: Pega o nome do metodo que invocou o metodo View
         * O atributo CallerMemberName veio com o C#5, e com ele a linguagem ganhou vários outros 
         * atributos de informações do método de chamada - ou, Caller Information.
         * 
         * Ao usar atributos de informações do chamador, você pode obter informações sobre o chamador de um método. 
         * Você pode obter o caminho do arquivo do código-fonte, o número da linha no código-fonte e o nome do membro do chamador.
         * Essas informações são úteis para fins de rastreamento, depuração e criação de ferramentas de diagnóstico.
         * 
         * CallerFilePathAttribute	
         * O caminho completo do arquivo de origem que contém o chamador. Esse é o caminho do arquivo no momento da compilação.	String
         * 
           CallerLineNumberAttribute	
           Número da linha no arquivo fonte no qual o método é chamado.	Integer

           CallerMemberNameAttribute	
           Nome do método ou da propriedade do chamador. Consulte Nomes dos membros mais adiante neste tópico.
         */
        protected string View([CallerMemberName]string nomeArquivo = null)
        {

            var type = GetType();
            var diretorioNome = type.Name.Replace("Controller", string.Empty);

            //Pego o meu recurso no assemblu
            var nomeCompletoResource = $"ByteBank.Portal.View.{diretorioNome}.{nomeArquivo}.html";
            var assembly = Assembly.GetExecutingAssembly();

            //Ele me retorna o stream
            var streamRecurso = assembly.GetManifestResourceStream(nomeCompletoResource);

            //Eu tranformo esse stream em string.
            var streamLeitura = new StreamReader(streamRecurso);

            //Agora eu tenho o valor em string, agora posso manipular esse texto injetnado o valor da variavel valorFinal.
            var textoPagina = streamLeitura.ReadToEnd();

            return textoPagina;
        }

        protected string View(object modelo, [CallerMemberName]string nomeArquivo = null)
        {
            // Usamos a outra sobrecarga deste método para recuperarmos o conteúdo
            // bruto de nossa view.
            var viewBruta = this.View(nomeArquivo);

            // Recuperamos todas as propriedades do modelo.
            var todasAsProprieadesDoModelo = modelo.GetType().GetProperties();

            // Criamos nossa expressão regular para  obter  os  valores  inseridos
            // entre um par de chaves duplas, como {{prop_1}} ou {{prop_2}}.
            var regex = new Regex("\\{{(.*?)\\}}");


            // O  método   Regex::Replace(string, MatchEvaluator)  executa   o
            // delegate  MatchEvaluator  para   cada   Match   encontrado   na
            // ViewBruta e substitui seu valor de  acordo  com  o  retorno  de
            // nossa expressão lambda.
            var viewProcessada = regex.Replace(viewBruta, (match) =>
            {
                // Verificamos com a ajuda do LinqPAD que o nome da propriedade
                // é acessível à partir do valor  do  segundo  grupo  de  nosso 
                // match.
                var nomeProprieade = match.Groups[1].Value;

                // Operamos nossa lista de todas as propriedades do modelo usando
                // o operador linq Single com segurança, pois sabemos  que  todas
                // as classes possuem propriedades com  nomes  únicos,  ou  seja,
                // os nomes de propriedades não se repetem.
                var propriedade = todasAsProprieadesDoModelo.Single(prop => prop.Name.Equals(nomeProprieade));

                // Com nosso PropertyInfo em mãos, basta  executarmos   o  método
                // PropertyInfo::GetValue(object) usando como argumento o  modelo
                // recebido em nossa nova sobrecarga View(object, string).

                // O delegate MatchEvaluator exige  como tipo  de retorno  uma
                // string. Como medida de  segurança,  usamos  o  null-coalescing
                // operator   para   não   termos    uma    exceção    do    tipo
                // NullReferenceException.
                return propriedade.GetValue(modelo)?.ToString();   //Null propagator operator expetion. Se o ojeto prorpriedate for nullo ele nao fara o toString.             
            });

            // Enfim, retornamos nossa view processada!
            return viewProcessada;
        }
    }
}
