using System;
using System.Collections.Generic;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.PontoE.Modelo
{
    [MapeamentoImplicito]
    public class ParametroConfiguracao : AbstractEntidade
    {
        public int Id { get; set; }
        public String sistema { get; set; }
        public String modulo { get; set; }
        public String item { get; set; }
        public String parametro { get; set; }
        public String descricao { get; set; }

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
