using System;
using System.Collections.Generic;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;

namespace Amon.PontoE.Modelo.Seguranca
{
    [MapeamentoImplicito]
    public class Usuario:AbstractEntidade
    {
        public int Id { get; set; }
        public String Login { get; set; }
        public String Nome { get; set; }
        public String Senha { get; set; }
        public String Email { get; set; }
        public String LoginRede { get; set; }
        public String Matricula { get; set; }
        public String IdsGrupo { get; set; }
        public String NomesGrupo { get; set; }
        public List<Operacao> Permissoes { get; set; }

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