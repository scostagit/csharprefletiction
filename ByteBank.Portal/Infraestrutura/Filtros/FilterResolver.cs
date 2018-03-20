using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteBank.Portal.Filtros;
using ByteBank.Portal.Infraestrutura.Binding;

namespace ByteBank.Portal.Infraestrutura.Filtros
{
    public class FilterResolver
    {
        public FilterResult VerificarFiltros(ActionBindingInfo actionBindInfo)
        {
            var methodInfo = actionBindInfo.MethodInfo;



            //Type attributeType: Type of attribute
            //bool inherit: Vai subir na arvore de herenaca de objectios sim ou nao:
            var atributos = methodInfo.GetCustomAttributes(typeof(FilterAttibute), false);

            foreach (FilterAttibute filtro in atributos)
                if (!filtro.PodeContinuar())
                    return new FilterResult(false);



            return new FilterResult(true);
        }
    }
}
