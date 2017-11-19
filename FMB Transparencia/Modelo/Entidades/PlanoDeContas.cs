using System;
using System.Collections.Generic;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.Modelo.Entidades
{
    [MapeamentoImplicito]
    public class PlanoDeContas : AbstractEntidade
    {
        public int CodCli { get; set; }
        public int CodContas { get; set; }
        public String Descricao { get; set; }
        public bool Analitica { get; set; }
        public int ContaReduz { get; set; }
        public Decimal ValorOriginal { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataExclusao { get; set; }

        #region Sobrecargas
        protected override List<String> obterChaves()
        {
            return new List<String> { "CodCli", "CodContas" };
        }

        protected override object obterInstancia()
        {
            return this;
        }
        #endregion

        public void DefinirChaves(int codCli, int codContas)
        {
            this.CodCli = codCli;
            this.CodContas = codContas;
        }
    }
}
