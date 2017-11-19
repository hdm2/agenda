using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.Nucleo.Persistencia;
using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Seguranca;
using Amon.PontoE.Persistencia.Seguranca;

namespace Amon.PontoE.Servico.Seguranca
{
    public class PerfilServico : ICrudServico<Perfil>
    {
        private readonly PerfilDAO dao;

        #region Construtores
        public PerfilServico()
        {
            dao = new PerfilDAO();
        }

        public PerfilServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<PerfilDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Perfil obter(int id)
        {
            return dao.obter(new Perfil { Id = id });
        }

        public IList<Perfil> obterTodos()
        {
            return dao.obterTodos();
        }

        public Perfil incluir(Perfil entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Perfil entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Perfil entidade)
        {
            dao.excluir(entidade);
        }

        #endregion
        
    }
}
