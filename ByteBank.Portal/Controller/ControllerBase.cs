using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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
        protected string View([CallerMemberName]string  nomeArquivo = null) {

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
    }
}
