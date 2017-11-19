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
    public class Operacao:AbstractEntidade
    {
        public int Id {get; set;}
        public String Nome { get; set; }
        public String Comentario { get; set; }
        public String Controller { get; set; }
        public String Acao { get; set; }
        public Boolean Leitura { get; set; }
        public Boolean Inclusao { get; set; }
        public Boolean Alteracao { get; set; }
        public Boolean Exclusao { get; set; }
        public String Icone { get; set; }

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
