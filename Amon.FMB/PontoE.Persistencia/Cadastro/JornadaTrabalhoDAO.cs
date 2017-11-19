using System;
using System.Linq;
using Amon.Persistencia;
using Amon.PontoE.Modelo.Cadastro;
using System.Collections.Generic;
using Amon.Persistencia.Construtor;

namespace Amon.PontoE.Persistencia.Cadastro
{
   public class JornadaTrabalhoDAO : SimplesAbstractDAO<JornadaTrabalho>
    {
        protected override string obterTabela()
        {
            return "JornadaTrabalhoBanpara";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public IList<JornadaTrabalho> obterNormalEAlternativas(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            //Consulta as jornadas, tanto alternativas, quanto a normal
            IList<JornadaTrabalho> jornadas = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL("SELECT idJornada id, inicio, fim, intervalo, minintervalo, maxintervalo, 0, 0, 0")
                .adicionaSQL(" FROM JornadaAlternativaBanpara")
                .adicionaSQL(String.Format(" WHERE idFuncionario = '{0}'", idFuncionario))
                .obterLista<JornadaTrabalho>();

            if (controleInterno)
                ad.fechaConexao();
           
            return jornadas;
        }

        public JornadaTrabalho IdentificarJornadaTrabalho(String idFuncionario, String horario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            //Consulta as jornadas, tanto alternativas, quanto a normal
            IList<JornadaTrabalho> jornadas = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL("SELECT idJornada id, inicio, fim, intervalo, minintervalo, maxintervalo, 0, 0, 0")
                .adicionaSQL(" FROM JornadaAlternativaBanpara")
                .adicionaSQL(String.Format(" WHERE idFuncionario = '{0}'", idFuncionario))
                .obterLista<JornadaTrabalho>();

            if (controleInterno)
                ad.fechaConexao();

            TimeSpan horaPrimeiraBatida = stringParaTimeSpan(horario);
            JornadaTrabalho definida = jornadas.First();
            foreach(JornadaTrabalho j in jornadas){
                double diferenca = Math.Abs(j.Inicio.Subtract(horaPrimeiraBatida).TotalSeconds);
                if (diferenca < Math.Abs(definida.Inicio.Subtract(horaPrimeiraBatida).TotalSeconds))
                    definida = j;
            }

            return definida;
        }

        private TimeSpan stringParaTimeSpan(String horario)
        {
            String[] p = horario.Split(':');
            List<int> valores = new List<int>();
            foreach (String s in p)
                valores.Add(Int32.Parse(s));
            return new TimeSpan(valores[0], valores[1], valores[2]);
        }

    }
}
