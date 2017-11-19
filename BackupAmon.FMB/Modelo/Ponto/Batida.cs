using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Modelo.Ponto
{
    [MapeamentoImplicito]
    public class Batida : AbstractEntidade
    {
        public int Id { get; set; }
        public String IdFuncionario { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Tipo { get; set; }
        public Boolean Entrada { get; set; }
        public string Ip { get; set; }
        public int? IdAutorizacao { get; set; }
        public String LoginAD { get; set; }

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