using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Amon.PontoE.Modelo.Ponto
{
    [MapeamentoImplicito]
    public class TipoAutorizacao : AbstractEntidade
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Cor { get; set; }
        public string Icone { get; set; }

        //Definem o intervalo em que o tipo de desbloqueio estará disponível. Se ambos forem NULL, o tipo ficará sempre disponível.
        public Nullable<DateTime> Inicio { get; set; }
        public Nullable<DateTime> Fim { get; set; }
      
        //Define a quantidade máxima de dias contidos no intervalo do desbloqueio (equivale a Fim - Início). Valor padrão definido na base de dados.
        public int MaxDiasDesbloqueaveis { get; set; } 
        
        //Se TRUE, indica que o desbloqueio deste tipo só pode ser cadastrado no dia corrente, não podendo ser programado para outros dias.
        public bool ApenasDiaCorrente { get; set; }
        
        //Define a antecedência máxima (em horas) com a qual um desbloqueio pode ser cadastrado.
        public int HorasAntecedentes { get; set; }

        //O atributo VisivelPara segue o seguinte padrão:
        /*
           * = Todos os gestores (é o default, definido na base)
           *-id_lotacao_1, id_lotacao_2, etc... = Todos os gestores, exceto os das lotações especificadas
           id_lotacao_1, id_lotacao_2, etc... = Apenas os gestores das lotações especificadas
        */
        public String VisivelPara { get; set; }

        //O atributo Limitado existe apenas para fazer o controle das datas de início e fim no formulário de manutenção
        [NaoMapear]
        public bool Limitado { get; set; }

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
