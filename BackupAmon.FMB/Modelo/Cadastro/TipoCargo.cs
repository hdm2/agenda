using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Modelo.Cadastro
{
    [MapeamentoImplicito]
   public class TipoCargo : AbstractEntidade
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        protected override List<string> obterChaves()
        {
            return new List<String> { "Id" };
        }

        protected override object obterInstancia()
        {
            return this;
        }
    }
}
