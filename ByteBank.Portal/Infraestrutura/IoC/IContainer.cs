using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    /*
     * IoC: Acronomo para inversao de controle.
     * 
     * Percebemos que ficar criando instância dos nossos serviços em todos os construtores das controllers
     * passou a ser um problema. Chegamos a qual solução para este problema?
     * 
     * Ao invés de criarmos os serviços nas controllers, vamos ter os serviços como dependência no construtor. 
     * Com o Reflection, vamos visitar os construtores, verificar suas dependências e instanciar as dependências de forma automática. 
       Correta! Este é mais um cenário em que o Reflection pode aumentar nossa produtividade e melhorar a qualidade do código.
     */
    public interface IContainer
    {
        //Metodo para registrar as intancias.
        void Registrar(Type tipoOrigem, Type TipoDestino);

        //metodo para recuperar o tipo
        object Recuperar(Type tipoOrigem);
    }
}
