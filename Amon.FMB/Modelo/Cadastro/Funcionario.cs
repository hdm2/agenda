using System;
using System.Collections.Generic;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.PontoE.Modelo.Cadastro
{
    [MapeamentoImplicito]
    public class Funcionario : AbstractEntidade
    {
        public String Id { get; set; }
        public String Nome { get; set; }
        public String LoginAD { get; set; }
        public String LotacaoId { get; set; }
        public String LotacaoDescricao { get; set; }
        public String IdJornada { get; set; }
        [Mapear("bPonto")]
        public Boolean NaoBatePonto { get; set; }
        public String Cnpj { get; set; }

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