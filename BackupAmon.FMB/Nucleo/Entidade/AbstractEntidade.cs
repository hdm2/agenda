using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Utils;

namespace Amon.Nucleo.Entidade
{
    [Serializable]
    public abstract class AbstractEntidade : IEntidade
    {
        //protected readonly bool AouD=new Delegate() {}
        protected String sufixoMapeamento;

        protected AbstractEntidade() { }

        abstract protected List<String> obterChaves();

        abstract protected Object obterInstancia();

        protected Dictionary<String, String> mapeamento;
        protected Dictionary<String, String> obterMapeamento()
        {
            if (mapeamento != null)
                return mapeamento;

            mapeamento = MapeamentoUtils.obterMapeamento(obterInstancia(), sufixoMapeamento);
            return mapeamento;

            //mapeamento = new Dictionary<string, string>();
            //Type tipo = obterInstancia().GetType();
            //MapeamentoImplicito mi = tipo.GetCustomAttribute<MapeamentoImplicito>(true);
            //PropertyInfo[] propriedades = tipo.GetProperties();
            //if (mi == null)
            //{
            //    foreach (PropertyInfo p in propriedades)
            //    {
            //        Mapear mapear = p.GetCustomAttribute<Mapear>(true);
            //        if (mapear == null)
            //            continue;
            //        mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco));
            //    }
            //}
            //else
            //{
            //    foreach (PropertyInfo p in propriedades)
            //    {
            //        NaoMapear naoMapear = p.GetCustomAttribute<NaoMapear>(true);
            //        if (naoMapear != null)
            //            continue;

            //        Mapear mapear = p.GetCustomAttribute<Mapear>(true);
            //        if (mapear == null)
            //            mapeamento.Add(p.Name, converteMapeamento(p.Name));
            //        else
            //            mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco));
            //    }
            //}
            //return mapeamento;
        }

        //private String converteMapeamento(String entrada)
        //{
        //    String saida = entrada;
        //    if (!String.IsNullOrEmpty(sufixoMapeamento))
        //    {
        //        saida = String.Format("{0}.{1}", sufixoMapeamento, entrada);
        //    }

        //    return saida;
        //}
        protected void converterValor(PropertyInfo pInfo, Type tipoCampo, Object vlObj)
        {
            if (vlObj == DBNull.Value) return;

            if (tipoCampo.IsSubclassOf(typeof(AbstractEntidade)))
            {
                Mapear mapear = pInfo.obterAtributo<Mapear>(true);
                if (mapear == null || String.IsNullOrEmpty(mapear.SufixoTabela))
                    return;

                ConstructorInfo constructorInfo = tipoCampo.GetConstructor(Type.EmptyTypes);
                if (constructorInfo == null)
                    throw new Exception(String.Format("Não foi possível instanciar {0}", tipoCampo.FullName));

                AbstractEntidade obj = (AbstractEntidade)constructorInfo.Invoke(null);

                obj.sufixoMapeamento = mapear.SufixoTabela;
                if (vlObj is IDataReader)
                {
                    obj.deReader(vlObj as IDataReader);
                    pInfo.SetValue(obterInstancia(), obj, null);
                    return;
                }
                if (typeof(Hashtable) == vlObj.GetType())
                {
                    obj.deHashtable(vlObj as Hashtable);
                    pInfo.SetValue(obterInstancia(), obj, null);
                    return;
                }
                if (typeof(DataRow) == vlObj.GetType())
                {
                    obj.deTable(vlObj as DataRow);
                    pInfo.SetValue(obterInstancia(), obj, null);
                    return;
                }
                return;
            }

            if (tipoCampo.Name.IndexOf("Nullable") > -1)
                tipoCampo = Nullable.GetUnderlyingType(pInfo.PropertyType);

            if ((tipoCampo == typeof(int)) && (!(vlObj is int)))
            {
                pInfo.SetValue(obterInstancia(), int.Parse(vlObj.ToString()), null);
            }
            else if ((tipoCampo == typeof(Char)) && (!(vlObj is char)))
            {
                pInfo.SetValue(obterInstancia(), Char.Parse(vlObj.ToString()), null);
            }
            else if (tipoCampo.IsEnum)
            {
                if (vlObj is string)
                    pInfo.SetValue(obterInstancia(), Enum.Parse(tipoCampo, vlObj.ToString(), true), null);
                else
                    pInfo.SetValue(obterInstancia(), Enum.ToObject(tipoCampo, vlObj), null);
            }
            else
            {
                pInfo.SetValue(obterInstancia(), vlObj, null);
            }
        }

        protected Dictionary<String, String> removerTransientes(Dictionary<String, String> mapa)
        {
            Dictionary<String, String> r = new Dictionary<string, string>();
            Type tipo = obterInstancia().GetType();
            foreach (String k in mapa.Keys)
            {
                PropertyInfo p = tipo.GetProperty(k, new Type[] { });
                if (p == null || p.GetCustomAttributes(typeof(Transiente), true).Length > 0)
                    continue;
                r.Add(k, mapa[k]);
            }
            return r;
        }

        /// <summary>
        /// Converte uma lista de dados de um IDataReader para uma lista da classe especificada.
        /// </summary>
        /// <typeparam name="Resultado">Tipo da classe que deve ser usada para a conversão.</typeparam>
        /// <param name="dr">Um IDataReader com dados para conversão.</param>
        /// <param name="maxLinhas">Se a consulta for muito grande pode-se optar por truncar o números de linhas que sera convertido com este paramentro.
        /// Zero (Padrão) para conversão total do resultado listado no IDataReader.</param>
        /// <returns>Retorna uma lista contendo as classes convertidas representando o resultado da consulta do IDataReader.</returns>
        public static List<Resultado> obterLista<Resultado>(IDataReader dr, int maxLinhas = 0)
        {
            Type tipo = typeof(Resultado);
            ConstructorInfo constructorInfo = tipo.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            if (constructorInfo == null)
                throw new Exception(String.Format("Não foi possível instanciar {0}", tipo.FullName));

            AbstractEntidade obj = (AbstractEntidade)constructorInfo.Invoke(null);
            return obj.obterLista(dr, maxLinhas).Cast<Resultado>().ToList();
        }

        public List<IEntidade> obterLista(IDataReader dr, int maxLinhas = 0)
        {
            List<IEntidade> r = new List<IEntidade>();
            Dictionary<String, PropertyInfo> dicTmp = new Dictionary<string, PropertyInfo>();
            Type tipo = obterInstancia().GetType();
            Dictionary<String, String> mapa = obterMapeamento();

            foreach (String k in mapa.Keys)
            {
                try
                {
                    dr.GetOrdinal(mapa[k]);
                }
                catch
                {
                    continue;
                }
                
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (!pInfo.CanWrite)
                    continue;
                dicTmp.Add(k, pInfo);
            }

            ConstructorInfo construtor = tipo.GetConstructor(Type.EmptyTypes);
            if (construtor == null)
                throw new Exception(String.Format("Não foi possível localizar um construtor vazio para o tipo {0}.",tipo.FullName));

            int i = 0;
            while (dr.Read())
            {
                if (maxLinhas > 0 && i == maxLinhas) break;

                AbstractEntidade ent = (AbstractEntidade)construtor.Invoke(null);

                foreach (String k in dicTmp.Keys)
                {
                    Object vlObj = dicTmp[k].PropertyType.IsSubclassOf(typeof(AbstractEntidade)) ? dr : dr[mapa[k]];
                    ent.converterValor(dicTmp[k], dicTmp[k].PropertyType, vlObj);
                }
                r.Add(ent);
                i++;
            }
            return r;
        }

        #region Implementação IEntidade
        public void deReader(IDataReader dr)
        {
            //if (DateTime.Today > new DateTime(2014, 7, 30))
            //    throw new Exception("VERSÃO DE DEMONSTRAÇÃ EXPIROU");

            Type tipo = obterInstancia().GetType();
            Dictionary<String, String> mapa = obterMapeamento();
            foreach (String k in mapa.Keys)
            {
                try
                {
                    dr.GetOrdinal(mapa[k]);
                }
                catch
                {
                    continue;
                }
                
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (dr[mapa[k]] == DBNull.Value || !pInfo.CanWrite)
                    continue;
                
                Type tipoCampo = pInfo.PropertyType;
                Object vlObj = tipoCampo.IsSubclassOf(typeof(AbstractEntidade)) ? dr : dr[mapa[k]];

                converterValor(pInfo, tipoCampo, vlObj);
            }
        }

        public void deTable(DataRow ln)
        {
            Type tipo = obterInstancia().GetType();
            Dictionary<String, String> mapa = obterMapeamento();

            foreach (String k in mapa.Keys)
            {
                if (!ln.Table.Columns.Contains(mapa[k])) continue;
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (ln[mapa[k]] == DBNull.Value || !pInfo.CanWrite)
                    continue;
                Type tipoCampo = pInfo.PropertyType;
                Object vlObj = ln[mapa[k]];

                converterValor(pInfo, tipoCampo, vlObj);
            }
        }

        public void deHashtable(Hashtable ht)
        {
            Type tipo = obterInstancia().GetType();
            Dictionary<String, String> mapa = obterMapeamento();

            foreach (String k in mapa.Keys)
            {
                if (!ht.ContainsKey(mapa[k])) continue;
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (ht[mapa[k]] == DBNull.Value || !pInfo.CanWrite)
                    continue;
                Type tipoCampo = pInfo.PropertyType;
                Object vlObj = ht[mapa[k]];

                converterValor(pInfo, tipoCampo, vlObj);
            }
        }

        public string instrucaoParaInsert(String campoAutoIncrementado)
        {
            StringBuilder sql = new StringBuilder("(");
            StringBuilder valores = new StringBuilder(" Values (");
            Dictionary<String, String> mapa = removerTransientes(obterMapeamento());

            if (campoAutoIncrementado != null) { 
                mapa.Remove(campoAutoIncrementado);
            }

            foreach (String k in mapa.Keys)
            {
                if (k.Equals(campoAutoIncrementado))
                    continue;

                String sep = ", ";
                if (k.Equals(mapa.Keys.Last()))
                    sep = ")";

                sql.Append(mapa[k]);
                sql.Append(sep);
                valores.AppendFormat("@{0}{1}", k, sep);
            }

            return sql.Append(valores).ToString();
        }

        public string instrucaoParaUpdate(String campoAutoIncrementado)
        {
            StringBuilder sql = new StringBuilder("set ");
            Dictionary<String, String> mapa = removerTransientes(obterMapeamento());

            foreach (String k in mapa.Keys)
            {
                if (k.Equals(campoAutoIncrementado)) continue;

                String sep = ", ";
                if (k.Equals(mapa.Keys.Last()))
                    sep = "";

                sql.AppendFormat("{0} = @{1}{2}", mapa[k], k, sep);
            }

            sql.Append(whereComChave());

            return sql.ToString();
        }

        public string whereComChave()
        {
            return whereComChave("");
        }

        public string whereComChave(String sufixoTabela)
        {
            Dictionary<String, String> mapa = obterMapeamento();

            StringBuilder sql = new StringBuilder(" where ");

            List<String> l = obterChaves();
            foreach (String k in l)
            {
                String sep = " and ";
                if (k.Equals(l.Last()))
                    sep = "";

                sql.AppendFormat("{0}{1} = @{2}{3}", sufixoTabela, mapa[k], k, sep);
            }

            return sql.ToString();
        }

        public Dictionary<String, Type> obterTiposMapeado()
        {
            Dictionary<String, String> mapa = obterMapeamento();
            Type tipo = obterInstancia().GetType();

            Dictionary<String, Type> r = new Dictionary<string, Type>();
            foreach (String k in mapa.Keys)
            {
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (pInfo == null)
                    throw new Exception(String.Format("Erro de mapeamento, propriedade {0} não foi encontrada na entidade {1}.", k, tipo.Name));

                if (pInfo.PropertyType.Name.IndexOf("Nullable") > -1)
                    r.Add(k, Nullable.GetUnderlyingType(pInfo.PropertyType));
                else
                    r.Add(k, pInfo.PropertyType);
            }

            return r;
        }

        public Dictionary<String, Type> obterTiposMapeadoChave()
        {
            List<String> chaves = obterChaves();
            Type tipo = obterInstancia().GetType();

            Dictionary<String, Type> r = new Dictionary<string, Type>();
            foreach (String k in chaves)
            {
                PropertyInfo pInfo = tipo.GetProperty(k, new Type[] { });
                if (pInfo.PropertyType.Name.IndexOf("Nullable") > -1)
                    r.Add(k, Nullable.GetUnderlyingType(pInfo.PropertyType));
                else
                    r.Add(k, pInfo.PropertyType);
            }

            return r;
        }
        
        [NaoMapear]
        public object this[string propriedade]
        {
            get
            {
                PropertyInfo pi = GetType().GetProperty(propriedade, new Type[] { });
                return pi == null ? null : pi.GetValue(this, null);
            }
        }
        #endregion
    }
}