using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Modelo.Seguranca
{
    [MapeamentoImplicito]
    public class UsuarioPerfil:AbstractEntidade
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }

        protected override List<string> obterChaves()
        {
            return null;
        }

        protected override object obterInstancia()
        {
            return this;
        }
    }
}
