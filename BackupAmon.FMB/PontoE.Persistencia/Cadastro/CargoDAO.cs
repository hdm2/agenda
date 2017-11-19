using Amon.Persistencia;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Persistencia.Cadastro
{
   public class CargoDAO : SimplesAbstractDAO<Cargo>
    {
        protected override string obterTabela()
        {
            return "Cargo";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }
    }
}
