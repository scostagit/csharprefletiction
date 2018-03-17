using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public class ManipuladorRequisicaoArquivo
    {
        public void Manipular(HttpListenerResponse resposta, string path)
        {
            //retornar  o nosso documento style.css
            var assembly = Assembly.GetExecutingAssembly(); //retornar o assembly que fez a chamada (ele mesmo) o assembly a ser retornar é o byteBank
                                                            //vamos acessar o recurso do css. vc nao tem acesso ao texto puro. voce tem acesso ao stream.
            var nomeResource = Utilidades.ConvertPathNomeAssembly(path);
            var resourceStream = assembly.GetManifestResourceStream(nomeResource);

            if (resourceStream == null)
            {
                resposta.StatusCode = 404;
                resposta.OutputStream.Close();
            }
            else
            {
                //A metodo GetManifestResourceStream deveolve um stream, que implementa a interface IDisposible.
                //é uma boa practia usar classes que implementam o IDisposble com o Using porque logo apos seu uso
                //o espaçao e liberado em memoria. Quando agente nao faz isso o .net mantem a referencia em memoria.
                using (resourceStream)
                {
                    var respostaConteudoBytes = new Byte[resourceStream.Length];
                    //vamos pegar os fluxo de dados da nossa mangueira para nosso baldinho(bytesResourse)
                    resourceStream.Read(respostaConteudoBytes, 0, (int)resourceStream.Length);

                    resposta.ContentType = Utilidades.ObterTipoDeConteudo(path);

                    //precisamos definir o status do codigo da requisição
                    resposta.StatusCode = 200; //Sucesso

                    //Informo ao IE, Chrome o tamnho de resposta que ele pode esperar
                    resposta.ContentLength64 = respostaConteudoBytes.Length;

                    //Vamos escrever nossa resposta http;
                    resposta.OutputStream.Write(respostaConteudoBytes, 0, respostaConteudoBytes.Length);

                    //precisamos fechar o fluxo
                    resposta.OutputStream.Close();
                }
               
            }
        }
    }
}
