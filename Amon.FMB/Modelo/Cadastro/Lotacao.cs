using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;

namespace Amon.PontoE.Modelo.Cadastro
{
    [MapeamentoImplicito]
    public class Lotacao : AbstractEntidade
    {
        public String Id { get; set; }
        public string Descricao { get; set; }
        public string Cnpj { get; set; }
        public string Cei { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }

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