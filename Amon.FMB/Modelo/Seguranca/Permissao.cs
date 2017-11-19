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
    public class Permissao:AbstractEntidade
    {
        public int IdPerfil { get; set; }
        public int IdAcao { get; set; }

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
