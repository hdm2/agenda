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
    public class JornadaTrabalho : AbstractEntidade
    {
        public String Id { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fim { get; set; }
        public int Intervalo { get; set; }
        [NaoMapear]
        public TimeSpan IntervaloTimeSpan
        {
            get { return new TimeSpan(0, Intervalo, 0); }
        }
        public TimeSpan MinIntervalo { get; set; }
        public TimeSpan MaxIntervalo { get; set; }
        public int Excecao { get; set; }
        public int Rep { get; set; }
        public int InterRepeticoes { get; set; }

        protected override List<string> obterChaves()
        {
            return new List<String> {"Id"};
        }

        protected override object obterInstancia()
        {
            return this;
        }
    }
}