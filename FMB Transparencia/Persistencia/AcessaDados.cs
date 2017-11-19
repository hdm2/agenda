using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Amon.Nucleo.Utils;

namespace Amon.Persistencia
{
    public class AcessaDados
    {
        public DbProviderFactory fabricaAcesso;
        protected DbConnection _con;
        protected DbTransaction _trans;
        protected bool _comTrans;

        private String sufixoCon = "";

        #region Construtores
        public AcessaDados()
        {
            iniciaClasse(false);
        }

        public AcessaDados(String sufixoConexao)
        {
            sufixoCon = sufixoConexao;
            iniciaClasse(false);
        }

        public AcessaDados(bool iniciaTransacao)
        {
            iniciaClasse(iniciaTransacao);
        }

        public AcessaDados(bool iniciaTransacao, String sufixoConexao)
        {
            sufixoCon = sufixoConexao;
            iniciaClasse(iniciaTransacao);
        }
        #endregion

        private void iniciaClasse(bool iniciaTransacao)
        {
            fabricaAcesso = DbProviderFactories.GetFactory(ConfigurationManager.AppSettings["Provedor" + sufixoCon]);
            _con = fabricaAcesso.CreateConnection();
            
            //DESCRIPTOGRAFA A STRING DE CONEXÃO
            //_con.ConnectionString = Crypto.Desencriptar(ConfigurationManager.AppSettings["ConnectionString" + sufixoCon]);
            _con.ConnectionString = ConfigurationManager.AppSettings["ConnectionString" + sufixoCon];

            _comTrans = iniciaTransacao;
            if (_comTrans)
            {
                _con.Open();
                _trans = _con.BeginTransaction();
            }
        }

        //***** Abrir e Fechar Conexão 
        public void abreConexao()
        {
            _con.Open();
        }
        public void fechaConexao()
        {
            _con.Close();
        }

        public bool conexaoFechada()
        {
            return _con.State == ConnectionState.Closed;
        }
        //***** Abrir e Fechar Conexão 

        //***** Comando de Transacao 
        public void iniciarTransacao()
        {
            if (_comTrans)
            {
                throw new Exception("Já existe uma transação em aberto.");
            }
            if (conexaoFechada())
                abreConexao();

            _comTrans = true;
            _trans = _con.BeginTransaction();
        }
        public void concluirTransacao()
        {
            if (!_comTrans)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            _trans.Commit();
            _con.Close();
            _comTrans = false;
        }
        public void cancelaTransacao()
        {
            if (!_comTrans)
            {
                throw new Exception("Não existe nenhuma transação em aberto.");
            }
            _trans.Rollback();
            _con.Close();
            _comTrans = false;
        }
        //***** Comandos de Transacao 

        #region "Funções para criação de parametros"
        public string obterNomeParametro(string nomeParametro)
        {
            return ConfigurationManager.AppSettings["InicioParam" + sufixoCon] + nomeParametro;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro)
        {
            DbParameter p = fabricaAcesso.CreateParameter();
            p.ParameterName = obterNomeParametro(nomeParametro);
            p.SourceColumnNullMapping = true;


            if (tipoParametro == DbType.Time && p is SqlParameter)
            {
                ((SqlParameter) p).SqlDbType = SqlDbType.Time;
            }
            else
            {
                p.DbType = tipoParametro;
            }
            return p;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro, object valorParametro)
        {
            DbParameter p = criaParametro(nomeParametro, tipoParametro);
            p.Value = valorParametro;
            return p;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro, object valorParametro, ParameterDirection direcaoParametro)
        {
            DbParameter p = criaParametro(nomeParametro, tipoParametro);
            p.Value = valorParametro;
            p.Direction = direcaoParametro;
            return p;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro, object valorParametro, ParameterDirection direcaoParametro, int tamanhoParametro)
        {
            DbParameter p = criaParametro(nomeParametro, tipoParametro);
            p.Value = valorParametro;
            p.Direction = direcaoParametro;
            p.Size = tamanhoParametro;
            return p;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro, ParameterDirection direcaoParametro)
        {
            DbParameter p = criaParametro(nomeParametro, tipoParametro);
            p.Direction = direcaoParametro;
            return p;
        }

        public DbParameter criaParametro(string nomeParametro, DbType tipoParametro, ParameterDirection direcaoParametro, int tamanhoParametro)
        {
            DbParameter p = criaParametro(nomeParametro, tipoParametro);
            p.Direction = direcaoParametro;
            p.Size = tamanhoParametro;
            return p;
        }
        #endregion

        public DbDataReader retornaDR(string instSQL)
        {
            return retornaDR(instSQL, new DbParameter[] { });
        }

        public DbDataReader retornaDR(string instSQL, DbParameter parametro)
        {
            return retornaDR(instSQL, new DbParameter[] { parametro });
        }
        public DbDataReader retornaDR(string instSQL, DbParameter parametro1, DbParameter parametro2)
        {
            return retornaDR(instSQL, new DbParameter[] { parametro1, parametro2 });
        }
        public DbDataReader retornaDR(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd;
            cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;

            if ((parametros != null) && (parametros.Length > 0))
                cmd.Parameters.AddRange(parametros);
            if (_comTrans)
                cmd.Transaction = _trans;
            return cmd.ExecuteReader();
        }

        public DbDataReader retornaSPDR(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd;
            cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;
            cmd.CommandType = CommandType.StoredProcedure;

            if ((parametros != null) && (parametros.Length > 0))
                cmd.Parameters.AddRange(parametros);
            if (_comTrans)
                cmd.Transaction = _trans;
            return cmd.ExecuteReader();
        }

        public DbParameterCollection retornaParametros(string instSQL)
        {
            return retornaParametros(instSQL, null);
        }

        public DbParameterCollection retornaParametros(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd;
            cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;

            if ((parametros != null))
                cmd.Parameters.AddRange(parametros);
            if (_comTrans)
                cmd.Transaction = _trans;
            cmd.ExecuteNonQuery();
            return cmd.Parameters;
        }

        public DbParameterCollection executaProcedureRetornaParametros(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd;
            cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = Int32.MaxValue;

            if ((parametros != null))
                cmd.Parameters.AddRange(parametros);
            if (_comTrans)
                cmd.Transaction = _trans;
            cmd.ExecuteNonQuery();
            return cmd.Parameters;
        }

        public int executaComando(string instSQL, DbParameter parametro)
        {
            return executaComando(instSQL, new DbParameter[] { parametro });
        }

        public int executaComando(string instSQL)
        {
            return executaComando(instSQL, new DbParameter[] { });
        }

        public int executaComando(string instSQL, params DbParameter[] parametros)
        {
            DbCommand cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;

            if ((parametros != null) && (parametros.Length > 0))
            {
                cmd.Parameters.AddRange(parametros);
            }
            if (_comTrans)
                cmd.Transaction = _trans;
            int r = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return r;
        }

        public DataTable retornaTabela(string nomeTabela, string instSQL)
        {
            return retornaTabela(nomeTabela, instSQL, false);
        }

        public DataTable retornaTabela(string nomeTabela, string instSQL, params DbParameter[] parametros)
        {
            return retornaTabela(nomeTabela, instSQL, false, parametros);
        }

        public DataTable retornaTabela(string nomeTabela, string instSQL, bool soEsquema, params DbParameter[] parametros)
        {
            DbCommand cmd = fabricaAcesso.CreateCommand();
            cmd.Connection = _con;
            cmd.CommandText = instSQL;
            cmd.CommandTimeout = int.MaxValue;

            if ((parametros != null))
                cmd.Parameters.AddRange(parametros);
            if (_comTrans)
                cmd.Transaction = _trans;
            DbDataAdapter DA;
            DA = fabricaAcesso.CreateDataAdapter();
            DA.SelectCommand = cmd;
            DataTable TB = new DataTable(nomeTabela);
            if (soEsquema)
            {
                DA.FillSchema(TB, SchemaType.Source);
            }
            else
            {
                DA.Fill(TB);
            }
            return TB;
        }
    }
}