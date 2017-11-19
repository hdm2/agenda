using System;
using System.Collections.Generic;
using System.Data;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.Nucleo.Seguranca
{
    [MapeamentoImplicito]
    public class Menu : AbstractEntidade, IEntidade
    {
        #region Construtores
        public Menu() { }
        public Menu(IDataReader dr)
        {
            this.deReader(dr);
        }
        #endregion

        #region Propriedades
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public String Descricao { get; set; }
        public String Tooltip { get; set; }
        public String Link { get; set; }
        public Boolean Ativo { get; set; }
        [Transiente]
        public Boolean TemPermissao { get; set; }
        [NaoMapear]
        public List<Menu> children { get; set; }
        [NaoMapear]
        public Menu Pai { get; set; }
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