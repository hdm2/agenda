using System;
using System.Collections.Generic;
using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Seguranca;

namespace Amon.PontoE.Persistencia.Seguranca
{
    public class UsuarioDAO:SimplesAbstractDAO<Usuario>
    {
        protected override string obterTabela()
        {
            return "Usuario";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        //MÉTODO OBSOLETO NA VERSÃO 1.5.2 (entregue em 05/04/2016)
        public IList<Usuario> obterPorLoginEmail(String login, String email)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Usuario> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL("select * from Usuario where ativo = 1")
                .adicionaSobCondicional(String.IsNullOrEmpty(login), " and login like @login", "login", String.Format("%{0}%", login))
                .adicionaSobCondicional(String.IsNullOrEmpty(email), " and email like @email", "email", String.Format("%{0}%", email))
                .adicionaSQL(" Order by login")
                .obterLista<Usuario>();

            if (controleInterno)
                ad.fechaConexao();

            return lista;
        }
    }
}