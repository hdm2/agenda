using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.Nucleo.Persistencia;
using Amon.Nucleo.Utils;
using Amon.PontoE.Modelo.Seguranca;
using Amon.PontoE.Persistencia.Seguranca;

namespace Amon.PontoE.Servico.Seguranca
{
    public class LogSgaServico
    {
        private readonly LogSgaDAO dao;

        #region Construtores
        public LogSgaServico()
        {
            dao = new LogSgaDAO();
        }

        public LogSgaServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<LogSgaDAO>();
        }
        #endregion

        public void registraLog(Usuario usuario, String txContxLogseg, String txOperaLog, String txIpUsuarRedeLog)
        {
            registraLog(usuario.Id, DateTime.Now, usuario.Nome, txContxLogseg, txOperaLog, txIpUsuarRedeLog, usuario.Login, ApoioUtils.getStrConfig("matricSGA"), usuario.LoginRede);
        }

        public void registraLog(int cdUsuarioLogseLog, DateTime dtHoraOperaLog, String txNomeUsuarLog,
            String txContxLogseg, String txOperaLog, String txIpUsuarRedeLog, String txLoginUsuarLog,
            String txMatriStemaLog, String txNomeUsuarRedeLog)
        {
            dao.registraLog(cdUsuarioLogseLog, dtHoraOperaLog, txNomeUsuarLog, txContxLogseg, txOperaLog, txIpUsuarRedeLog, txLoginUsuarLog, txMatriStemaLog, txNomeUsuarRedeLog);
        }
    }
}