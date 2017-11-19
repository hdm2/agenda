using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Entidade;
using Amon.Nucleo.Utils;

namespace Amon.Persistencia.Construtor
{
    public class SqlConstrutor<Tipo> : ISqlConstrutor
    {
        protected enum TipoUniao
        {
            Inner,
            Left,
            Right
        }

        protected class Uniao
        {
            private const String inner = " inner join ";
            private const String left = " left join ";
            private const String right = " right join ";

            private readonly SqlConstrutor<Tipo> sqlConstrutor;
            private readonly IEntidade instancia;
            private readonly Mapear mapear;

            public Uniao(SqlConstrutor<Tipo> construtor, String nome, Type tipo, String tabela, String apelido = null)
            {
                sqlConstrutor = construtor;
                Nome = nome;
                Tipo = tipo;
                Tabela = tabela;
                Apelido = apelido ?? tabela;
                ConstructorInfo constructorInfo = Tipo.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                instancia = (IEntidade) constructorInfo.Invoke(null);


                mapear = sqlConstrutor.entidade.GetType().GetProperty(nome).obterAtributo<Mapear>(true);//.GetRuntimeProperty(nome).GetCustomAttribute<Mapear>();
                if (mapear == null)
                    throw new Exception(String.Format("Propriedade {0} da entidade {1} não possui a assinatura Mapear.", Nome, sqlConstrutor.entidade.GetType().FullName));
                Mapeamento = MapeamentoUtils.obterMapeamento(instancia, Apelido);
            }

            public TipoUniao TipoUniao { get; set; }
            public String Nome { get; private set; }
            public Type Tipo { get; private set; }
            public String Tabela { get; private set; }
            public String Apelido { get; private set; }
            public String CondicaoAdicional { get; set; }
            public Dictionary<string, string> Mapeamento { get; private set; }


            public override string ToString()
            {
                StringBuilder r = new StringBuilder();
                switch (TipoUniao)
                {
                    case TipoUniao.Inner:
                        r.Append(inner);
                        break;
                    case TipoUniao.Left:
                        r.Append(left);
                        break;
                    case TipoUniao.Right:
                        r.Append(right);
                        break;
                }


                string chave = instancia.obterTiposMapeadoChave().Keys.First();

                r.AppendFormat("{0} as {1} on ({2}.{3} = {1}.{4} {5}) ", Tabela, Apelido,
                               sqlConstrutor.apelidoPrincipal, mapear.ColunaBanco,
                               chave, CondicaoAdicional);

                return r.ToString();
            }
        }

        protected readonly IEntidade entidade;
        protected readonly String tabelaPrincipal;
        protected readonly String apelidoPrincipal;

        protected String sqlSelect;
        protected String sqlFrom;

        protected List<Uniao> listaJoins;

        public Dictionary<string, string> Mapeamento { get; private set; }

        protected SqlConstrutor(IEntidade entidade, String tabela, String apelido = null)
        {
            this.entidade = entidade;
            tabelaPrincipal = tabela;
            apelidoPrincipal = String.IsNullOrEmpty(apelido)
                                   ? entidade.GetType().Name
                                   : apelido;

            listaJoins = new List<Uniao>();
            Mapeamento = MapeamentoUtils.obterMapeamento(entidade, apelidoPrincipal);
        }
        
        public static SqlConstrutor<Tipo> iniciar(String tabela, String apelido = null)
        {
            Type tipo = typeof (Tipo);
            ConstructorInfo constructorInfo =
                tipo.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                                    new Type[0], null);
            IEntidade instancia = (IEntidade) constructorInfo.Invoke(null);
            return new SqlConstrutor<Tipo>(instancia, tabela, apelido);
        }

        public static ISqlConstrutor iniciar(IEntidade entidade, String tabela, String apelido = null)
        {
            return new SqlConstrutor<Tipo>(entidade, tabela, apelido);
        }


        public ISqlConstrutor innerJoin<Prop>(Expression<Func<Tipo, Prop>> express, String tabela, String apelido = null,
                                       String condicionalAdicional = null)
        {
            String propriedade = (express.Body as MemberExpression).Member.Name;
            return adicionaUniao(TipoUniao.Inner, propriedade, tabela, apelido, condicionalAdicional);
        }

        public ISqlConstrutor innerJoin(String propriedade, String tabela, String apelido = null,
                                       String condicionalAdicional = null)
        {
            return adicionaUniao(TipoUniao.Inner, propriedade, tabela, apelido, condicionalAdicional);
        }

        public ISqlConstrutor leftJoin(String propriedade, String tabela, String apelido = null,
                                      String condicionalAdicional = null)
        {
            return adicionaUniao(TipoUniao.Left, propriedade, tabela, apelido, condicionalAdicional);
        }

        public ISqlConstrutor rightJoin(String propriedade, String tabela, String apelido = null,
                                       String condicionalAdicional = null)
        {
            return adicionaUniao(TipoUniao.Right, propriedade, tabela, apelido, condicionalAdicional);
        }

        protected ISqlConstrutor adicionaUniao(TipoUniao tipoUniao, String propriedade, String tabela,
                                              String apelido = null, String condicionalAdicional = null)
        {
            Type tipo = entidade.obterTiposMapeado()[propriedade];
            PropertyInfo prop = entidade.GetType().GetProperty(propriedade);
            if (prop == null)
                throw new Exception(String.Format("Propriedade {0} da entidade {1} não possui a assinatura Mapear.",
                                                  propriedade, entidade.GetType().FullName));

            Uniao u = new Uniao(this, propriedade, tipo, tabela, apelido)
                {
                    TipoUniao = tipoUniao,
                    CondicaoAdicional = condicionalAdicional
                };

            listaJoins.Add(u);

            return this;
        }

        public String obterSelect()
        {
            construirSQLSelect();
            return sqlSelect;
        }

        private void construirSQLSelect()
        {
            StringBuilder select = new StringBuilder("select ");
            select.Append(apelidoPrincipal);
            select.Append(".*");

            foreach (Uniao u in listaJoins)
            {
                foreach (KeyValuePair<string, string> m in u.Mapeamento)
                {
                    string nomeColuna = m.Value.Remove(0, m.Value.IndexOf(".") + 1);
                    select.AppendFormat(", {0}.{1} [{2}]", u.Apelido, nomeColuna, m.Value);
                }
            }

            this.sqlSelect = select.ToString();
        }

        public String obterFrom()
        {
            construirSQLFrom();
            return sqlFrom;
        }

        private void construirSQLFrom()
        {
            StringBuilder from = new StringBuilder(" from ");
            from.Append(tabelaPrincipal);
            from.Append(" as ");
            from.AppendLine(apelidoPrincipal);

            foreach (Uniao u in listaJoins)
            {
                from.AppendLine(u.ToString());
            }

            this.sqlFrom = from.ToString();
        }

        public String obterSqlCompleto()
        {
            StringBuilder sql = new StringBuilder();
            construirSQLSelect();
            sql.AppendLine(sqlSelect);
            construirSQLFrom();
            sql.AppendLine(sqlFrom);
            return sql.ToString();
        }

        public String obterNomeMapeado<Prop>(Expression<Func<Tipo, Prop>> express)
        {
            return obterNomeMapeado(express.Body.ToString().Replace(string.Format("{0}.", express.Parameters[0].Name), ""));
        }

        public String obterNomeMapeado(string propriedade)
        {
            String[] props = propriedade.Split('.');
            if (props.Length == 1)
                return Mapeamento[propriedade];

            Uniao uniao = listaJoins.FirstOrDefault(j => j.Nome == props[0]);
            if (uniao == null)
                throw new Exception(String.Format("A propriedade {0} não foi adicionada como Join.", props[0]));

            return uniao.Mapeamento[props[1]];
        }

        public String obterNomeMapeadoParametro(string propriedade)
        {
            return obterNomeMapeado(propriedade).Replace('.', '_');
        }
    }
}