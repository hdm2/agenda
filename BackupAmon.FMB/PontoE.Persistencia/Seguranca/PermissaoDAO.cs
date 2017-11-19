using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Persistencia.Seguranca
{
  public  class PermissaoDAO : SimplesAbstractDAO<Permissao>
    {
        protected override string obterTabela()
        {
            return "Permissao";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return null;
        }
    }
}
