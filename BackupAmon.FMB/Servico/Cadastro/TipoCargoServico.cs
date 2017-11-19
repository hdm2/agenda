using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.PontoE.Persistencia.Cadastro;
using Amon.Nucleo.Persistencia;

namespace Amon.PontoE.Servico.Cadastro
{
   public class TipoCargoServico : ICrudServico<TipoCargo>
    {
        private readonly TipoCargoDAO dao;

        #region Construtores
        public TipoCargoServico()
        {
            dao = new TipoCargoDAO();
        }

        public TipoCargoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<TipoCargoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public TipoCargo obter(int id)
        {
            return dao.obter(new TipoCargo { Id = id });
        }

        public IList<TipoCargo> obterTodos()
        {
            return dao.obterTodos();
        }

        public TipoCargo incluir(TipoCargo entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(TipoCargo entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(TipoCargo entidade)
        {
            dao.excluir(entidade);
        }

        #endregion
        
    }
}
