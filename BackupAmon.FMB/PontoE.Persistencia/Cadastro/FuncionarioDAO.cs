using Amon.Persistencia;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Cadastro;

namespace Amon.PontoE.Persistencia.Cadastro
{
    public class FuncionarioDAO : SimplesAbstractDAO<Funcionario>
    {
        protected readonly SqlConstrutor<Funcionario> sqlConstrutor;

        #region Construtores
        public FuncionarioDAO()
            : base()
        {
            sqlConstrutor = SqlConstrutor<Funcionario>.iniciar(obterTabela());
        }

        public FuncionarioDAO(AcessaDados ad)
            : base(ad)
        {
            sqlConstrutor = SqlConstrutor<Funcionario>.iniciar(obterTabela());
        }
        #endregion

        protected override string obterTabela()
        {
            return "FuncionarioBanpara";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public IList<Funcionario> obterPorLotacao(String idLotacao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Funcionario> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sqlConstrutor.obterSelect())
                .adicionaSQL(sqlConstrutor.obterFrom())
                .adicionaSQL(" Where {0} in ('{1}')", sqlConstrutor.obterNomeMapeado(f => f.LotacaoId), idLotacao)
                .obterLista<Funcionario>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Funcionario> obterPorMatricula(String idMatricula)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Funcionario> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sqlConstrutor.obterSelect())
                .adicionaSQL(sqlConstrutor.obterFrom())
                .adicionaSQL(" Where {0} in ('{1}')", sqlConstrutor.obterNomeMapeado(f => f.Id), idMatricula)
                .obterLista<Funcionario>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public IList<Funcionario> obterDaMesmaLotacaoPorMatricula(String id, String lotacao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            String condicao = String.Format(" Where {0} in ('{1}')", sqlConstrutor.obterNomeMapeado(f => f.Id), id);

            if (lotacao != null)
                condicao += String.Format(" and LotacaoId = '{0}'", lotacao);

            IList<Funcionario> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sqlConstrutor.obterSelect())
                .adicionaSQL(sqlConstrutor.obterFrom())
                .adicionaSQL(condicao)
                .obterLista<Funcionario>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }

        public Funcionario obterPorLoginAD(string loginAd)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Funcionario> lista = AcessaDadosConstrutor.iniciar(ad)
                                                            .adicionaSQL(sqlConstrutor.obterSelect())
                                                            .adicionaSQL(sqlConstrutor.obterFrom())
                                                            .adicionaSQL(" Where ")
                                                            .adicionaSqlComParametro(sqlConstrutor, sqlConstrutor.obterNomeMapeado(f => f.LoginAD), loginAd)
                                                            .obterLista<Funcionario>();
            if (controleInterno)
                ad.fechaConexao();

            return lista.FirstOrDefault();
        }

        public IList<Funcionario> verificaSeEGerente(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            DbDataReader dr = ad.retornaDR(String.Format("SELECT * FROM ResponsavelLotacao WHERE idFuncionario = '{0}'", idFuncionario));

            IList<Funcionario> funcionario = new List<Funcionario>();
            if (dr.HasRows)
                funcionario = obterPorMatricula(idFuncionario);

            if (controleInterno)
                ad.fechaConexao();

            return funcionario;
        }
    }
}