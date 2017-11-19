using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;

namespace Amon.PontoE.Modelo.Ponto
{
    [MapeamentoImplicito]
    public class Autorizacao : AbstractEntidade
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fim { get; set; }
        public String IdFuncionario { get; set; }
        public String IdUsuarioAutorizador { get; set; }
        public int IdTipo { get; set; }
        public string Observacao { get; set; }
        [NaoMapear]
        public bool Notificado { get; set; }
        [NaoMapear]
        public DateTime DataFim { get; set; }

        protected override List<string> obterChaves()
        {
            return new List<String> { "Id" };
        }

        protected override object obterInstancia()
        {
            return this;
        }

        //Cria e retorna uma cópia do objeto passado como "original"
        public static Autorizacao operator / (Autorizacao original, Autorizacao copia)
        {
            copia.Data = original.Data;
            copia.Inicio = original.Inicio;
            copia.Fim = original.Fim;
            copia.IdFuncionario = original.IdFuncionario;
            copia.IdUsuarioAutorizador = original.IdUsuarioAutorizador;
            copia.IdTipo = original.IdTipo;
            copia.Observacao = original.Observacao;
            return copia;
        }
    }
}
