using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.Nucleo.Persistencia;
using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Seguranca;
using Amon.PontoE.Persistencia.Seguranca;

namespace Amon.PontoE.Servico.Seguranca
{
    public class UsuarioServico:ICrudServico<Usuario>
    {
        private readonly UsuarioDAO dao;

        #region Construtores
        public UsuarioServico()
        {
            dao = new UsuarioDAO();
        }

        public UsuarioServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<UsuarioDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Usuario obter(int id)
        {
            return dao.obter(new Usuario {Id = id});
        }

        public IList<Usuario> obterTodos()
        {
            return dao.obterTodos();
        }

        public Usuario incluir(Usuario entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Usuario entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Usuario entidade)
        {
            dao.excluir(entidade);
        }
        #endregion

        //MÉTODO OBSOLETO NA VERSÃO 1.5.2 (entregue em 05/04/2016)
        public IList<Usuario> obterPorLoginEmail(String login, String email)
        {
            return dao.obterPorLoginEmail(login, email);
        }
    }
}