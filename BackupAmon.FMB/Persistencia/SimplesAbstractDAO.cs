using System;
using System.Reflection;
using Amon.Nucleo.Persistencia;

namespace Amon.Persistencia
{
    public abstract class SimplesAbstractDAO : ISimplesDao
    {
        protected AcessaDados ad;

        #region "Construtores"
        protected SimplesAbstractDAO()
        {
            ad = new AcessaDados();
        }

        protected SimplesAbstractDAO(AcessaDados ac)
        {
            ad = ac;
        }
        #endregion

        #region Implementação de ISimplesDao
        public void abreConexao()
        {
            ad.abreConexao();
        }

        public void fechaConexao()
        {
            ad.fechaConexao();
        }

        public void iniciarTransacao()
        {
            ad.iniciarTransacao();
        }

        public void cancelarTransacao()
        {
            ad.cancelaTransacao();
        }

        public void concluirTransacao()
        {
            ad.concluirTransacao();
        }

        public object obterOutroDao(Type t)
        {
            Type[] a = new Type[1] { typeof(AcessaDados) };
            Object[] b = new Object[1] { ad };
            ConstructorInfo cinf = t.GetConstructor(a);

            if (cinf == null)
                return null;
            
            return cinf.Invoke(b);
        }

        public Resultado obterOutroDao<Resultado>() where Resultado : ISimplesDao
        {
            Type[] a = new Type[1] { typeof(AcessaDados) };
            Object[] b = new Object[1] { ad };
            ConstructorInfo cinf = typeof(Resultado).GetConstructor(a);

            if (cinf == null)
                return default(Resultado);// null;

            return (Resultado)cinf.Invoke(b);
        }
        #endregion
    }
}