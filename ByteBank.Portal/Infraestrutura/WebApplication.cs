using ByteBank.Portal.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public class WebApplicationAlura
    {
        private readonly string[] _prefixos;

        /// <summary>
        /// Prefixo: URL que irão ser ouvidas pelo nossso httplistener
        /// </summary>
        /// <param name="prefixos"></param>
        public WebApplicationAlura(string[] prefixos)
        {
            //se prefixos for nullos sera lançado uma execçãoi.
            if (prefixos == null)
                throw new ArgumentNullException(nameof(prefixos)); //nameof: novo metodo do C# que irá retornar o nome da variavel.


            _prefixos = prefixos;
        }

        public void Iniciar()
        {
            //looping infinito para ficar ouvindo diversas requisições.
            while (true)
                ManipularRequisicao();
        }

        private void ManipularRequisicao()
        {
            //httpLisener: Classe que vai ficar ouvindo as requisições http.
            //prefixo: começo das urls que ele vai ouvir.
            var httpListener = new HttpListener();

            //Precisamos passar os prefixmo para a class httplistener
            foreach (string prefixo in this._prefixos)
            {
                httpListener.Prefixes.Add(prefixo);
            }

            //Start: iniciando o servidor
            httpListener.Start();

            //Precisamos obter o contexto:
            //quando nossa aplicação chega nesse ponto ela trava, fica esperando uma requisção.
            var contexto = httpListener.GetContext();

            //preciso de dois objetos. um de resposta e um de requisição.
            //objecto que representa nossa requisição
            var requisicao = contexto.Request;

            //ojecto que representa nossa resposta
            var resposta = contexto.Response;

            //Nao podemos devolder um texto puro, isso nao vai rolar. Tenho que transformar minha string em um fluxo de bytes.
            //O objexto Stream e um fluxo de bytes:
            //Conteudo a ser devolvido.
            var respostaConteudo = "hello World";

            //Transformando minha string em um array de bytes. Agora eu posso mandar esse array de bytes para nosso cliente para nossa rede.
            var respostaConteudoBytes = Encoding.UTF8.GetBytes(respostaConteudo);

            //AbsolutePath: indica qual diretorio a applicação esta tentando acessar
            var path = requisicao.Url.AbsolutePath;

            if (Utilidades.EhArquivo(path))
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
                    respostaConteudoBytes = new Byte[resourceStream.Length];
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
            else if (path == "/Cambio/MXN")
            {
                var controller = new CambioController();
                var paginaConteudo = controller.MXN();
                var bufferPagina = Encoding.UTF8.GetBytes(paginaConteudo);
                resposta.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                resposta.StatusCode = 200;
                resposta.ContentType = "text/html; charset=utf-8";
                // resposta.ContentLength64 = bufferPagina.Length;
                resposta.OutputStream.Close();
            }
            else if (path == "/Cambio/USD")
            {
                var controller = new CambioController();
                var paginaConteudo = controller.MXN();
                var bufferPagina = Encoding.UTF8.GetBytes(paginaConteudo);
                resposta.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                resposta.StatusCode = 200;
                resposta.ContentType = "text/html; charset=utf-8";
                // resposta.ContentLength64 = bufferPagina.Length;
                resposta.OutputStream.Close();
            }

            httpListener.Stop();
        }

        /*
         * Assembly:
         * Podemos dizer para o compilador do C# embutir os arquivos na DLL ou seja no assembly.
         * Fazemos isso com o botão direito no arquivo -> propriedades -> build action -> embed reouse(embutir no recurso)
         * depois de fazermos isso, esses arquivos sao acessados atraves do objecto ASSEMBLY
         */

        /*
         * O que é o Stream?
         * É um fluxo de dados. Imagine que cada arquivo ou variavel seja um balde com varios bytes, agora temos um 
         * balde com um giga? como fariamos. nao vamos levar isso de uma vez. Vamos usar a mangueira um fluxo de dados.
         * Steam nada mais é que um array de bytes para transmitir arquivos muito extensoes de pouco em pouco. voce
         * pode ir trabalhando byte por byte.
         */

        /*
         * Ao recuperar recursos embutidos em nosso Assembly, sempre lidamos com Streams. 
         * Por que streams são importantes até quando lidamos com recursos de Assemblies?
         * 
         * É possível embutir arquivos realmente grandes em nossos Assemblies. Deste modo, recuperar 
         * do disco rígido ou outra fonte um arquivo muito grande, pode sacrificar a performance da
         * aplicação e outros processos em execução na máquina. Para evitar este tipo de problema, 
         * devemos trabalhar com fluxos de dados.
         * 
         * Correto. Trabalhar com fluxos de dados ao invés de lidar com todo o conteúdo do arquivo de 
         * uma só vez nos traz benefícios quando temos documentos muito grandes para processar.
         */
    }
}
