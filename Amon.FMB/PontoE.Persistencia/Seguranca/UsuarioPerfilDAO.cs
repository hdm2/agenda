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
   public class UsuarioPerfilDAO : SimplesAbstractDAO<UsuarioPerfil>
    {
        protected override string obterTabela()
        {
            return "UsuarioPerfil";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return null;
        }
    }
}
