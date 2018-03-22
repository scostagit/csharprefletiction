using ByteBank.Portal.Controller;
using ByteBank.Portal.Infraestrutura.IoC;
using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Service.Cartao;
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
        private readonly IContainer _container = new ContainerSimples();

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

            this.Configurar();
        }

        public void Iniciar()
        {
            //looping infinito para ficar ouvindo diversas requisições.
            while (true)
                ManipularRequisicao();
        }

        private void Configurar()
        {
            //this._container.Registrar(typeof(ICambioService), typeof(CambioTestService));
            //this._container.Registrar(typeof(ICartaoService), typeof(CartaoService));

            this._container.Registrar<ICambioService, CambioTestService>();
            this._container.Registrar<ICartaoService, CartaoService>();

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
            var path = requisicao.Url.PathAndQuery;

            if (Utilidades.EhArquivo(path))
            {
                var manipulardor = new ManipuladorRequisicaoArquivo();

                manipulardor.Manipular(resposta, path);
            }
            else
            {
                var manipuladorController = new ManipuladorRequisicaoController(this._container);
                manipuladorController.Manipular(resposta, path);
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
