using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    public class ContainerSimples : IContainer
    {
        private readonly Dictionary<Type, Type> _mapaDeTipos = new Dictionary<Type, Type>();

        // Registrar o mapeamento de um tipo para outro,
        // como, por exemplo, mapear a dependência de
        // ICambioService para a implementação CambioTesteService
        public void Registrar(Type tipoOrigem, Type tipoDestino)
        {
            if (_mapaDeTipos.ContainsKey(tipoOrigem))
                throw new InvalidOperationException("Tipo já mapeado!");

            VerificarHierarquiaOuLancarExcecao(tipoOrigem, tipoDestino);

            _mapaDeTipos.Add(tipoOrigem, tipoDestino);
        }

        // Verificar se tipoDestino herda ou implementa tipoOrigem
        private void VerificarHierarquiaOuLancarExcecao(Type tipoOrigem, Type tipoDestino)
        {
            if (tipoOrigem.IsInterface)
            {
                // Se o tipoOrigem representar o tipo de uma Interface,
                // devemos verificar de o tipoDestino  implementa  este
                // tipo.
                var tipoDestinoImplementaInterface =
                    tipoDestino
                        .GetInterfaces()
                        .Any(tipoInterface => tipoInterface == tipoOrigem);

                // Caso não encontremos a interface tipoOrigem na  coleção
                // de interfaces implementadas pelo tipoDestino, temos uma
                // inconsistência e devemos lançar uma exceção!
                if (!tipoDestinoImplementaInterface)
                    throw new InvalidOperationException("O tipo destino não implementa a interface");
            }
            else
            {
                // Se o tipoOrigem representar o tipo  de  uma  Classe,
                // devemos verificar de o tipoDestino  herda este tipo.
                var tipoDestinoHerdaTipoOrigem = tipoDestino.IsSubclassOf(tipoOrigem);

                // Se o tipoDestino não for subClasse do tipoOrigem,
                // devemos lançar uma exceção.
                if (!tipoDestinoHerdaTipoOrigem)
                    throw new InvalidOperationException("O tipo destino não herda o tipo de origem");
            }
        }

        //estamos recuperamndo uma instancai de tipo origem
        public object Recuperar(Type tipoOrigem)
        {
            //vamos verifcar se eu tenho o tipo origem mapeando
            var tipoOrigemMapeado = this._mapaDeTipos.ContainsKey(tipoOrigem);
            if (tipoOrigemMapeado) {
                //Recupera do tipoOrigem para esse tipoDestino
                var tipoDestino = this._mapaDeTipos[tipoOrigem];
                //agora tenho o tipodestino preciso recperar ele.
                //vamos chamar recursivamente 
                return Recuperar(tipoDestino);
            }

            //precisamos recuperar os construturoes
            var construstores = tipoOrigem.GetConstructors(); //GetConstructors: Nao garante a ordem dos construtores.

            //Recuperar o construtor sem parametro.
            var construtorSemPrametro = 
                construstores.FirstOrDefault(construtor => construtor.GetParameters().Any() == false);

            if (construtorSemPrametro != null)
            {
                //passando o paramtros vazios. O retorno do Invoce é um object. Sendo assim podemos retorna-lo.
                var instancia = construtorSemPrametro.Invoke(new object[0]);

                return instancia;
            }

            //Se estamos aqui é porque nao temos contrustruroes sem parametros.
            //Pega o primeiro construtor com parametro.
            var construtorQueVamosUsar = construstores[0];
            var parametrosDoConstrutor = construtorQueVamosUsar.GetParameters();
            var valoresDeParamtros = new object[parametrosDoConstrutor.Count()];

            for (int i = 0; i < parametrosDoConstrutor.Count(); i++)
            {
                var parametro = parametrosDoConstrutor[i];
                var tipoParametros = parametro.ParameterType;

                valoresDeParamtros[i] = Recuperar(tipoParametros);
            }

            var instanciaDeConstrutorSemParametro = construtorQueVamosUsar.Invoke(valoresDeParamtros);
            return instanciaDeConstrutorSemParametro;
           
        }
    }
}


