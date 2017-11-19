using System;
using System.Collections.Generic;
using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.Modelo.Entidades;

namespace Amon.DAO.PlanoContas
{
    public class PlanoDeContasDAO : SimplesAbstractDAO<PlanoDeContas>
    {
        #region Construtores e implementações
        protected readonly SqlConstrutor<PlanoDeContas> sqlConstrutor;

        public PlanoDeContasDAO() : base()
        {
            sqlConstrutor = SqlConstrutor<PlanoDeContas>.iniciar(obterTabela());
        }

        public PlanoDeContasDAO(AcessaDados ad) : base(ad)
        {
            sqlConstrutor = SqlConstrutor<PlanoDeContas>.iniciar(obterTabela());
        }

        protected override string obterTabela()
        {
            return "PlanoContas";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return null;
        }
        #endregion

        public IList<PlanoDeContas> obterPorChaves(int codCli, int codConta)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE codCli = @codCli AND codContas = @codConta")
                                        .adicionaParametro("codCli", codCli)
                                        .adicionaParametro("codConta", codConta)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorCodigoCliente(int codCliente)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE codCli = @codCliente
                                                        ORDER BY descricao")
                                        .adicionaParametro("codCliente", codCliente)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorCodigoConta(int codConta)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE codContas = @codConta
                                                        ORDER BY descricao")
                                        .adicionaParametro("codConta", codConta)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorDataCriacao(DateTime? dataCriacao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE dataCriacao BETWEEN @dataCriacao AND DATEADD(second, -1, DATEADD(day, 1, @dataCriacao))
                                                        ORDER BY dataCriacao DESC")
                                        .adicionaParametro("dataCriacao", dataCriacao)
                                        .obterLista<PlanoDeContas>();
            
            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorDataExclusao(DateTime? dataExclusao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE dataExclusao BETWEEN @dataExclusao AND DATEADD(second, -1, DATEADD(day, 1, @dataExclusao))
                                                        ORDER BY dataExclusao DESC")
                                        .adicionaParametro("dataExclusao", dataExclusao)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorDescricao(String descricao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE descricao LIKE '%' + @descricao + '%'
                                                        ORDER BY descricao")
                                        .adicionaParametro("descricao", descricao)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }

        public IList<PlanoDeContas> ObterPorBooleanAnaliticas(bool analitica)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<PlanoDeContas> planosDeConta = AcessaDadosConstrutor.iniciar(ad)
                                        .adicionaSQL(@" SELECT *
                                                        FROM PlanoContas
                                                        WHERE analitica = @analitica
                                                        ORDER BY descricao")
                                        .adicionaParametro("analitica", analitica)
                                        .obterLista<PlanoDeContas>();

            if (controleInterno)
                ad.fechaConexao();

            return planosDeConta;
        }
    }
}