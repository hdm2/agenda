using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Amon.Persistencia;

namespace Amon.PontoE.Persistencia.Seguranca
{
    public class LogSgaDAO : SimplesAbstractDAO
    {
        #region Construtores
        public LogSgaDAO() : base() { }

        public LogSgaDAO(AcessaDados ad) : base(ad) { }
        #endregion

        public void registraLog(int cdUsuarioLogseLog, DateTime dtHoraOperaLog, String txNomeUsuarLog,
            String txContxLogseg, String txOperaLog, String txIpUsuarRedeLog, String txLoginUsuarLog,
            String txMatriStemaLog, String txNomeUsuarRedeLog)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();
            
            ad.executaComando(@"INSERT INTO [TB_LOGSE_LOG]
           ([CD_USUARIO_LOGSE_LOG]
           ,[DT_HORA_OPERA_LOG]
           ,[TX_NOME_USUAR_LOG]
           ,[TX_CONTX_LOGSEG]
           ,[TX_OPERA_LOG]
           ,[TX_IP_USUAR_REDE_LOG]
           ,[TX_LOGIN_USUAR_LOG]
           ,[TX_MATRI_STEMA_LOG]
           ,[TX_NOME_STEMA_LOG]
           ,[IN_TIPO_LOG], [TX_NOME_USUAR_REDE_LOG])
     VALUES
           (@CD_USUARIO_LOGSE_LOG
           ,@DT_HORA_OPERA_LOG
           ,@TX_NOME_USUAR_LOG
           ,@TX_CONTX_LOGSEG
           ,@TX_OPERA_LOG
		   ,@TX_IP_USUAR_REDE_LOG
           ,@TX_LOGIN_USUAR_LOG
           ,@TX_MATRI_STEMA_LOG
           ,'SRP', 1, @TX_NOME_USUAR_REDE_LOG)"
                , ad.criaParametro("CD_USUARIO_LOGSE_LOG", DbType.Int32, cdUsuarioLogseLog)
                , ad.criaParametro("DT_HORA_OPERA_LOG", DbType.DateTime, dtHoraOperaLog)
                , ad.criaParametro("TX_NOME_USUAR_LOG", DbType.String, txNomeUsuarLog)
                , ad.criaParametro("TX_CONTX_LOGSEG", DbType.String, txContxLogseg)
                , ad.criaParametro("TX_OPERA_LOG", DbType.String, txOperaLog)
                , ad.criaParametro("TX_IP_USUAR_REDE_LOG", DbType.String, txIpUsuarRedeLog)
                , ad.criaParametro("TX_NOME_USUAR_REDE_LOG", DbType.String, txNomeUsuarRedeLog)
                , ad.criaParametro("TX_LOGIN_USUAR_LOG", DbType.String, txLoginUsuarLog)
                , ad.criaParametro("TX_MATRI_STEMA_LOG", DbType.String, txMatriStemaLog));

            if (controleInterno)
                ad.fechaConexao();
        }
    }
}