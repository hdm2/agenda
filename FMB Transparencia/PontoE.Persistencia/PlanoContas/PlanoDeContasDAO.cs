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




        //Consulta básica de listagem
        public IList<PlanoDeContas> ObterPorDataCriacao(DateTime dataCriacao)
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
    }
}