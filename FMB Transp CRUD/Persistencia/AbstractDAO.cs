using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using Amon.Nucleo.Persistencia;
using Amon.Nucleo.Utils;

namespace Amon.Persistencia
{
    public abstract class SimplesAbstractDAO<E> : SimplesAbstractDAO, ISimplesDao where E : AbstractEntidade
    {
        protected abstract String obterTabela();
        protected abstract String obterCampoAutoIncrementado();

        #region "Construtores"
        protected SimplesAbstractDAO() : base() { }

        protected SimplesAbstractDAO(AcessaDados ad) : base(ad) { }
        #endregion

        public virtual E obter(E chave)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            List<DbParameter> param = new List<DbParameter>();
            Dictionary<String, Type> tipos = chave.obterTiposMapeadoChave();
            foreach (String k in tipos.Keys)
            {
                param.Add(ad.criaParametro(k, ConverteUtils.converteParaDbType(tipos[k]), typeof(E).GetProperty(k, new Type[] { }).GetValue(chave, null)));
            }

            StringBuilder sql = new StringBuilder("Select * from ");
            sql.Append(obterTabela());
            sql.Append(chave.whereComChave());

            E entidade = null;
            IDataReader dr = ad.retornaDR(sql.ToString(), param.ToArray());
            if (dr.Read())
            {
                entidade = (E)typeof(E).GetConstructors()[0].Invoke(null);
                entidade.deReader(dr);
            }
            dr.Close();

            if (controleInterno)
                ad.fechaConexao();

            return entidade;
        }

        public virtual IList<E> obterTodos()
        {
            StringBuilder sql = new StringBuilder("SELECT * FROM ");
            sql.Append(obterTabela());

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IDataReader dr = ad.retornaDR(sql.ToString());
            IList<E> r = AbstractEntidade.obterLista<E>(dr);
            dr.Close();

            if (controleInterno)
                ad.fechaConexao();

            return r;
        }

        public virtual E incluir(E entidade)
        {
            Dictionary<String, DbParameter> param = criarParamatros(entidade);
            string campoAutoIncrementado = obterCampoAutoIncrementado();

            //remove campo autoincrementado
            if (!String.IsNullOrEmpty(campoAutoIncrementado) && param.ContainsKey(campoAutoIncrementado))
            {
                param.Remove(obterCampoAutoIncrementado());
            }

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            StringBuilder sql = new StringBuilder("Insert into ");
            sql.Append(obterTabela());
            sql.Append(entidade.instrucaoParaInsert(campoAutoIncrementado));
            try
            {
                if (!String.IsNullOrEmpty(campoAutoIncrementado))
                {
                    sql.Append("select @idRetorno = @@identity");
                    DbParameter idRetorno = ad.criaParametro("idRetorno", DbType.Int32);
                    idRetorno.Direction = ParameterDirection.Output;
                    param.Add("idRetorno", idRetorno);

                    ad.retornaParametros(sql.ToString(), param.Values.ToArray());
                    PropertyInfo pInfo = entidade.GetType().GetProperty(campoAutoIncrementado, new Type[] { });
                    if (pInfo.PropertyType == typeof(Decimal))
                        pInfo.SetValue(entidade, ConverteUtils.sempreConverteDecimal(idRetorno.Value), null);
                    else
                        pInfo.SetValue(entidade, ConverteUtils.sempreConverteInt32(idRetorno.Value), null);
                }
                else
                {
                    ad.executaComando(sql.ToString(), param.Values.ToArray());
                }
            }
            catch (Exception ex)
            {
                String msg = String.Format("Falha ao tentar incluir registro no banco de dados.\n{0}", ex.Message);
                if (ex.Message.IndexOf("FK_") > -1)
                    msg = String.Format("Falha de integridade referencial na tentativa de incluir o registro.\n{0}", ex.Message);
                if (ex.Message.IndexOf("PK_") > -1 || ex.Message.IndexOf("UK_") > -1)
                    msg = "Registro que está sendo incluído já existe no banco de dados.";
                throw new Exception(msg, ex);
            }

            if (controleInterno)
                ad.fechaConexao();

            return entidade;
        }

        public virtual void atualizar(E entidade)
        {
            Dictionary<String, DbParameter> param = criarParamatros(entidade);

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            StringBuilder sql = new StringBuilder("Update ");
            sql.Append(obterTabela());
            sql.Append(" ");
            sql.Append(entidade.instrucaoParaUpdate(obterCampoAutoIncrementado()));

            try
            {
                ad.executaComando(sql.ToString(), param.Values.ToArray());
            }
            catch (Exception ex)
            {
                String msg = String.Format("Falha ao tentar atualizar registro no banco de dados.\n{0}", ex.Message);
                if (ex.Message.IndexOf("FK_") > -1)
                    msg = String.Format("Falha de integridade referencial na tentativa de atualizar o registro.\n{0}", ex.Message);
                if (ex.Message.IndexOf("PK_") > -1 || ex.Message.IndexOf("UK_") > -1)
                    msg = "A tentativa de atualização do registro provocará uma duplicidade no banco de dados.";
                throw new Exception(msg, ex);
            }

            if (controleInterno)
                ad.fechaConexao();
        }

        public virtual void excluir(E entidade)
        {
            Type tipo = typeof(E);

            List<DbParameter> param = new List<DbParameter>();
            Dictionary<String, Type> tipos = entidade.obterTiposMapeadoChave();
            foreach (String k in tipos.Keys)
            {
                param.Add(ad.criaParametro(k, ConverteUtils.converteParaDbType(tipos[k]), tipo.GetProperty(k, new Type[] { }).GetValue(entidade, null)));
            }

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            StringBuilder sql = new StringBuilder("Delete from ");
            sql.Append(obterTabela());
            sql.Append(entidade.whereComChave());

            try
            {
                ad.executaComando(sql.ToString(), param.ToArray());
            }
            catch (Exception ex)
            {
                String msg = String.Format("Falha ao tentar excluir registro no banco de dados.\n{0}", ex.Message);
                if (ex.Message.IndexOf("FK_") > -1)
                    msg = String.Format("Falha de integridade referencia na tentativa de excluir o registro, possívelmente ele já esta em uso pelo sistema.\n{0}", ex.Message);
                throw new Exception(msg, ex);
            }

            if (controleInterno)
                ad.fechaConexao();
        }

        protected Dictionary<String, DbParameter> criarParamatros(IEntidade entidade)
        {
            Type tipo = entidade.GetType();

            Dictionary<String, DbParameter> param = new Dictionary<String, DbParameter>();
            Dictionary<String, Type> tipos = entidade.obterTiposMapeado();
            foreach (String k in tipos.Keys)
            {
                PropertyInfo p = tipo.GetProperty(k, new Type[] { });
                if (p == null || p.GetCustomAttributes(typeof(Transiente), true).Length > 0)
                    continue;

                

                if (p.GetValue(entidade, null) == null)
                    param.Add(k, ad.criaParametro(k, ConverteUtils.converteParaDbType(tipos[k]), DBNull.Value));
                else if (tipos[k].IsEnum)
                    param.Add(k, ad.criaParametro(k, DbType.Int32, (int) p.GetValue(entidade, null)));
                else if (tipos[k].IsSubclassOf(typeof(AbstractEntidade)))
                {//Se for uma entidade: 1-obtem seu mapeamento de chaves, 2-Pega a primeira chave e passa para o mapeamento
                    IEntidade tmp = (IEntidade) p.GetValue(entidade);
                    Dictionary<string, Type> chv = tmp.obterTiposMapeadoChave();
                    String priChv = chv.Keys.First();
                    param.Add(k, ad.criaParametro(k, ConverteUtils.converteParaDbType(chv[priChv]), tmp[priChv]));
                }
                else if (tipos[k] == typeof (TimeSpan))
                {
                    TimeSpan ts = (TimeSpan) p.GetValue(entidade, null);
                    //DateTime data = new DateTime(1981, 1, 30, ts.Hours, ts.Minutes, ts.Seconds);
                    param.Add(k, ad.criaParametro(k, ConverteUtils.converteParaDbType(tipos[k]), ts));
                }
                else
                    param.Add(k, ad.criaParametro(k, ConverteUtils.converteParaDbType(tipos[k]), p.GetValue(entidade, null)));
            }

            return param;
        }

    }
}