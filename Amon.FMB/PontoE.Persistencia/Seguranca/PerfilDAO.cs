using Amon.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.PontoE.Modelo.Seguranca;

namespace Amon.PontoE.Persistencia.Seguranca
{
   public class PerfilDAO : SimplesAbstractDAO<Perfil>
    {
        protected override string obterTabela()
        {
            return "Perfil";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }
    }
}
