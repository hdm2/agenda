using System.Data;
using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amon.PontoE.Persistencia.Cadastro
{
   public class LotacaoDAO : SimplesAbstractDAO<Lotacao>
    {
       protected readonly SqlConstrutor<Lotacao> sqlConstrutor;

       #region Construtores
       public LotacaoDAO() : base()
       {
           sqlConstrutor = SqlConstrutor<Lotacao>.iniciar(obterTabela());
       }

       public LotacaoDAO(AcessaDados ad): base(ad)
       {
           sqlConstrutor = SqlConstrutor<Lotacao>.iniciar(obterTabela());
       }
       #endregion

        protected override string obterTabela()
        {
            return "LotacaoBanpara";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

       public IList<Lotacao> obterLotacoesNaoBloqueadas()
       {
           bool controleInterno = ad.conexaoFechada();
           if (controleInterno)
               ad.abreConexao();

           IList<Lotacao> r = AcessaDadosConstrutor.iniciar(ad)
               .adicionaSQL(@"SELECT *
                              FROM LotacaoBanpara l
                              inner join NaoBloqueiaLotacao nb on l.id = nb.idLotacao
                              order by l.descricao").obterLista<Lotacao>();

           if (controleInterno)
               ad.fechaConexao();

           return r;
       }

       public Boolean verificaSeBloqueada(String id)
       {
           bool controleInterno = ad.conexaoFechada();
           if (controleInterno)
               ad.abreConexao();

           IList<Lotacao> r = AcessaDadosConstrutor.iniciar(ad)
               .adicionaSQL(@"SELECT *
                                FROM LotacaoBanpara l
                                inner join NaoBloqueiaLotacao nb on l.id = nb.idLotacao
                                where l.id like @id
                                order by l.descricao")
                            .adicionaParametro("id", id)
                            .obterLista<Lotacao>();
           if (controleInterno)
               ad.fechaConexao();

           return r.Count > 0;
       }

       public void deleteNaoBloqueada(String id)
       {
           bool controleInterno = ad.conexaoFechada();
           if (controleInterno)
               ad.abreConexao();

           ad.executaComando(@"delete nb from NaoBloqueiaLotacao nb
                                where nb.idLotacao = @id", ad.criaParametro("id", DbType.String, id));

           if (controleInterno)
               ad.fechaConexao();
       }

       public void insereNaoBloqueada(String id)
       {
           bool controleInterno = ad.conexaoFechada();
           if (controleInterno)
               ad.abreConexao();

           ad.executaComando(@"insert into NaoBloqueiaLotacao
                                (idLotacao) values (@id)", ad.criaParametro("id", DbType.String, id));

           if (controleInterno)
               ad.fechaConexao();
       }

       public override IList<Lotacao> obterTodos()
       {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM {0} order by descricao", obterTabela());

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Lotacao> r = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sql.ToString()).obterLista<Lotacao>();

            if (controleInterno)
                ad.fechaConexao();

            return r;
       }

       public IList<Lotacao> obterTodosExcetoNaoBloqueadas()
       {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Lotacao> r = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"SELECT *
                                FROM LotacaoBanpara l
                                where id not in (select idLotacao from NaoBloqueiaLotacao)")
                                .obterLista<Lotacao>();

            if (controleInterno)
                ad.fechaConexao();

            return r;
       }

       public IList<Lotacao> obterLotacoesPorFuncionario(String idFuncionario, DateTime data)
       {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IDataReader dr = AcessaDadosConstrutor.iniciar(ad)
                            .adicionaSQL(@"select idLotacao
                                            from ResponsavelLotacao 
                                            where idFuncionario = @idFuncionario
                                            and @data between inicio and isnull(fim, '99990101')")
                            .adicionaParametro("idFuncionario", idFuncionario)
                            .adicionaParametro("data", data.Date)
                            .obterDataReader();

            String campoId = sqlConstrutor.obterNomeMapeado(l => l.Id);
            StringBuilder sb = new StringBuilder("(");
            sb.AppendFormat("{0} like ''", campoId);

            while (dr.Read())
                sb.AppendFormat(" or {0} like '%{1}%'", campoId, dr.GetString(0));
            
            dr.Close();

            sb.Append(")");
            String where = sb.ToString();

            IList<Lotacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sqlConstrutor.obterSqlCompleto())
                .adicionaSQL(@" where {0}", where)
                .adicionaSQL(" order by descricao")
                .obterLista<Lotacao>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }
    }
}