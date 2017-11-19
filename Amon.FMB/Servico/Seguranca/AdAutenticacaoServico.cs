using System;
using System.Configuration;
using System.DirectoryServices;

namespace Amon.PontoE.Servico.Seguranca
{
    public static class AdAutenticacaoServico
    {
        private static String dominio = ConfigurationManager.AppSettings["dominioAD"];

        public static String Autenticar(String usuario, String senha)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry(String.Format("LDAP://{0}", dominio), usuario, senha);
            try
            {
                String nome = directoryEntry.Name;
                return directoryEntry.Username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }
}