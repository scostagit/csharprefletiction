using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Filtros
{
    //por convenção do .net a class deve ter o sufixo da classe que esta herandado.
    public class ApenasHorarioComercialFilterAttibute : FilterAttibute
    {
        public override bool PodeContinuar()
        {
            //var hora = DateTime.Now.Hour;

            //return hora >= 9 && hora < 16;

            return true;
        }
    }
}
