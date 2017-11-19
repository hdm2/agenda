using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amon.PontoE.Persistencia.Seguranca
{
    public class OperacaoDAO : SimplesAbstractDAO<Operacao>
    {
        protected readonly SqlConstrutor<Operacao> sqlConstrutor;

        #region Construtores
        public OperacaoDAO() : base()
        {
            sqlConstrutor = SqlConstrutor<Operacao>.iniciar(obterTabela());
        }

        public OperacaoDAO(AcessaDados ad) : base(ad)
        {
            sqlConstrutor = SqlConstrutor<Operacao>.iniciar(obterTabela());
        }

        #endregion

        protected override string obterTabela()
        {
            return "Acao";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public Operacao obterPorNome(String nome)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Operacao> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(sqlConstrutor.obterSqlCompleto())
                .adicionaSQL(" Where ")
                .adicionaSqlComParametro(sqlConstrutor, sqlConstrutor.obterNomeMapeado(o => o.Nome), nome)
                .obterLista<Operacao>();
            if (controleInterno)
                ad.fechaConexao();

            return lista.FirstOrDefault();
        }
    }
}