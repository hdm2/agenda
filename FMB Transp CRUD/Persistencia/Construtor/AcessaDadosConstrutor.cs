using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using Amon.Nucleo.Entidade;
using Amon.Nucleo.Utils;

namespace Amon.Persistencia.Construtor
{
    public class AcessaDadosConstrutor
    {
        protected readonly AcessaDados ad;
        protected StringBuilder sql;
        protected List<DbParameter> parametros;

        protected AcessaDadosConstrutor()
        {
            ad = new AcessaDados();
            sql = new StringBuilder();
            parametros = new List<DbParameter>();
        }

        protected AcessaDadosConstrutor(AcessaDados ad)
        {
            this.ad = ad;
            sql = new StringBuilder();
            parametros = new List<DbParameter>();
        }

        public static AcessaDadosConstrutor iniciar(AcessaDados ad = null)
        {
            return ad == null ? new AcessaDadosConstrutor() : new AcessaDadosConstrutor(ad);
        }

        public AcessaDadosConstrutor adicionaSQL(String strSql, params Object[] args)
        {
            sql.AppendFormat(strSql, args);
            return this;
        }

        public AcessaDadosConstrutor adicionaParametro(string nomeParametro, object valorParametro)
        {
            return adicionaParametro(nomeParametro, ConverteUtils.converteParaDbType(valorParametro.GetType()), valorParametro);
        }

        public AcessaDadosConstrutor adicionaParametro(string nomeParametro, DbType tipoParametro, object valorParametro)
        {
            parametros.Add(ad.criaParametro(nomeParametro, tipoParametro, valorParametro));
            return this;
        }

        public AcessaDadosConstrutor adicionaSqlComParametroSobCondicional(Boolean condicional,
                                                                           ISqlConstrutor sqlConstrutor,
                                                                           String propriedade, object valorParametro)
        {
            if (!condicional)
                return this;

            return adicionaSqlComParametro(sqlConstrutor, propriedade, valorParametro);
        }

        public AcessaDadosConstrutor adicionaSqlComParametro(ISqlConstrutor sqlConstrutor, String propriedade, object valorParametro)
        {
            DbType tipoParam = ConverteUtils.converteParaDbType(valorParametro.GetType());
            return adicionaSqlComParametro(sqlConstrutor, propriedade, tipoParam, valorParametro);
        }

        public AcessaDadosConstrutor adicionaSqlComParametro(ISqlConstrutor sqlConstrutor, String propriedade, DbType tipoParametro, object valorParametro)
        {
            String nomeColuna = propriedade.IndexOf(".") > -1 ? propriedade : sqlConstrutor.obterNomeMapeado(propriedade);
            String nomeParam = nomeColuna.Replace('.', '_');
            sql.AppendFormat(" {0} = {1} ", nomeColuna, ad.obterNomeParametro(nomeParam));
            adicionaParametro(nomeParam, tipoParametro, valorParametro);
            return this;
        }

        public AcessaDadosConstrutor adicionaParametro(string nomeParametro, DbType tipoParametro, object valorParametro, ParameterDirection direcao)
        {
            parametros.Add(ad.criaParametro(nomeParametro, tipoParametro, valorParametro, direcao));
            return this;
        }

        public AcessaDadosConstrutor adicionaSobCondicional(Boolean condicional, String strSql, String nomeParametro = null, object valorParametro = null)
        {
            if (!condicional)
                return this;

            sql.Append(strSql);
            return String.IsNullOrEmpty(nomeParametro)
                       ? this
                       : adicionaParametro(nomeParametro, valorParametro);
        }

        public IDataReader obterDataReader(Boolean procedure = false)
        {
            return procedure
                       ? ad.retornaSPDR(sql.ToString(), parametros.ToArray())
                       : ad.retornaDR(sql.ToString(), parametros.ToArray());
        }

        public DataTable obterDataTable(String nomeTabela = "TB")
        {
            return ad.retornaTabela(nomeTabela, sql.ToString(), parametros.ToArray());
        }

        public IList<Resultado> obterLista<Resultado>(Boolean procedure = false)
        {
            IDataReader dr = obterDataReader(procedure);
            IList<Resultado> r = AbstractEntidade.obterLista<Resultado>(dr);
            dr.Close();
            return r;
        }

        public DbParameterCollection obterParametros(Boolean procedure = false)
        {
            return procedure
                       ? ad.executaProcedureRetornaParametros(sql.ToString(), parametros.ToArray())
                       : ad.retornaParametros(sql.ToString(), parametros.ToArray());
        }
    }

}