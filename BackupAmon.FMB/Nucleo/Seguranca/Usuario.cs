using System;
using System.Collections.Generic;
using System.Data;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.Nucleo.Seguranca
{
    [MapeamentoImplicito]
    public class Usuario : AbstractEntidade, IEntidade
    {
        #region Construtores
        public Usuario() { }
        public Usuario(IDataReader dr)
        {
            this.deReader(dr);
        }
        #endregion

        #region Propriedades
        public int Id { get; set; }
        public String Email { get; set; }
        public String Senha { get; set; }
        public String Nome { get; set; }
        public Boolean Ativo { get; set; }
        public int Nivel { get; set; }
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