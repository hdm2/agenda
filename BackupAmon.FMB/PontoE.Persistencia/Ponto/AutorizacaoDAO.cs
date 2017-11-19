using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Ponto;
using System;
using System.Collections.Generic;

namespace Amon.PontoE.Persistencia.Ponto
{
   public class AutorizacaoDAO : SimplesAbstractDAO<Autorizacao>
    {
        protected override string obterTabela()
        {
            return "Autorizacao";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public IList<Autorizacao> obterTodasDoFuncionario(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Autorizacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL("select * from Autorizacao where idFuncionario = @idFuncionario order by data DESC")
                .adicionaParametro("idFuncionario", idFuncionario)
                .obterLista<Autorizacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Autorizacao> obterTodasDoDiaPorFuncionario(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Autorizacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL("select * from Autorizacao where idFuncionario = @idFuncionario and data = @dataHoje order by data")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("dataHoje", DateTime.Today)
                .obterLista<Autorizacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Autorizacao> ObterAutorizacoesDoDiaNaoNotificadas(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Autorizacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select *
                               from Autorizacao
                               where idFuncionario = @idFuncionario
                                    and data = @data
                                    and notificado = 0")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("data", DateTime.Today)
                .obterLista<Autorizacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Autorizacao> marcarComoNotificada(int idAutorizacao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            ad.executaComando(String.Format("UPDATE Autorizacao SET notificado = 1 WHERE id = {0}", idAutorizacao));


//            Autorizacao autorizacao = AcessaDadosConstrutor.iniciar(ad)
//                .adicionaSQL(@"select *
//                                           from Autorizacao
//                                           where idFuncionario = @id")
//                .adicionaParametro("id", idAutorizacao)
//                .obterLista<Autorizacao>().First();

//            autorizacao.Notificado = true;
//            atualizar(autorizacao);

            if (controleInterno)
                ad.fechaConexao();

            return new List<Autorizacao>();
        }

        public IList<Autorizacao> obterTodasPorData(String id, DateTime inicio, DateTime fim)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Autorizacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select * 
                                from Autorizacao a
                                where idFuncionario = @idFuncionario
                                and a.data between @dataInicio and @dataFim")
                .adicionaParametro("idFuncionario", id)
                .adicionaParametro("dataInicio", inicio.Date)
                .adicionaParametro("dataFim", fim.Date)
                .obterLista<Autorizacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Autorizacao> obterTodasAPartirDaData(String id, DateTime data)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Autorizacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select * 
                                from Autorizacao a
                                where idFuncionario = @idFuncionario
                                and a.data >= @data")
                .adicionaParametro("idFuncionario", id)
                .adicionaParametro("data", data)
                .obterLista<Autorizacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }
    }
}