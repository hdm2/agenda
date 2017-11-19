using System;
using System.Collections.Generic;
using System.Data;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.Nucleo.Seguranca
{
    [MapeamentoImplicito]
    public class Perfil : AbstractEntidade, IEntidade
    {
        #region Construtores
        public Perfil() { }
        public Perfil(IDataReader dr)
        {
            this.deReader(dr);
        }
        #endregion

        #region Propriedades
        public int Id { get; set; }
        public String Descricao { get; set; }
        #endregion

        #region Implementação de AbstractEntidade
        protected override List<string> obterChaves()
        {
            List<String> r = new List<string>();
            r.Add("Id");
            return r;
        }

        protected override object obterInstancia()
        {
            return this;
        }
        #endregion
    }
}