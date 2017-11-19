using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amon.PontoE.Persistencia;
using Amon.PontoE.Persistencia.Seguranca;
using Amon.Nucleo.Persistencia;

namespace Amon.PontoE.Servico.Seguranca
{
    public class OperacaoServico : ICrudServico<Operacao>
    {
        private readonly OperacaoDAO dao;

        #region Construtores
        public OperacaoServico()
        {
            dao = new OperacaoDAO();
        }

        public OperacaoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<OperacaoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Operacao obter(int id)
        {
            return dao.obter(new Operacao { Id = id });
        }

        public IList<Operacao> obterTodos()
        {
            return dao.obterTodos();
        }

        public Operacao incluir(Operacao entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Operacao entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Operacao entidade)
        {
            dao.excluir(entidade);
        }

        #endregion

        public Operacao obterPorNome(String nome)
        {
            return dao.obterPorNome(nome);
        }
    }
}