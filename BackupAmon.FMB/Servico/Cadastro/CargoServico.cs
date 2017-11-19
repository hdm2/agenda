using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amon.PontoE.Persistencia.Seguranca;
using Amon.Nucleo.Persistencia;
using Amon.PontoE.Persistencia.Cadastro;

namespace Amon.PontoE.Servico.Cadastro
{
    public class CargoServico : ICrudServico<Cargo>
    {
        private readonly CargoDAO dao;

        #region Construtores
        public CargoServico()
        {
            dao = new CargoDAO();
        }

        public CargoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<CargoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Cargo obter(int id)
        {
            return dao.obter(new Cargo { Id = id });
        }

        public IList<Cargo> obterTodos()
        {
            return dao.obterTodos();
        }

        public Cargo incluir(Cargo entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Cargo entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Cargo entidade)
        {
            dao.excluir(entidade);
        }

        #endregion
        
    }
}
