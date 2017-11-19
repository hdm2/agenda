using System;
using System.Collections.Generic;
using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo;

namespace Amon.PontoE.Persistencia
{
    public class ParametroConfiguracaoDAO : SimplesAbstractDAO<ParametroConfiguracao>
    {
       protected readonly SqlConstrutor<ParametroConfiguracao> sqlConstrutor;

       #region Construtores
       public ParametroConfiguracaoDAO() : base()
       {
           sqlConstrutor = SqlConstrutor<ParametroConfiguracao>.iniciar(obterTabela());
       }

       public ParametroConfiguracaoDAO(AcessaDados ad)
           : base(ad)
       {
           sqlConstrutor = SqlConstrutor<ParametroConfiguracao>.iniciar(obterTabela());
       }
       #endregion

       protected override string obterTabela()
       {
           return "ParametrosConfiguracao";
       }

       protected override string obterCampoAutoIncrementado()
       {
           return "id";
       }

       public IList<ParametroConfiguracao> obterParametroConfiguracao(String sistema, String modulo, String item)
       {
           bool controleInterno = ad.conexaoFechada();
           if (controleInterno)
               ad.abreConexao();

           IList<ParametroConfiguracao> r = AcessaDadosConstrutor.iniciar(ad)
               .adicionaSQL(@"SELECT *
                              FROM " + 
                              obterTabela() +
                              String.Format(" WHERE sistema = '{0}' AND modulo = '{1}' AND item = '{2}'", sistema, modulo, item))
               .obterLista<ParametroConfiguracao>();

           if (controleInterno)
               ad.fechaConexao();

           return r;
       }
    }
}
