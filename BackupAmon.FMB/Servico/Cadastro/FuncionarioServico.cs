using System;
using Amon.Nucleo.Servico;
using System.Collections.Generic;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Persistencia.Cadastro;
using Amon.Nucleo.Persistencia;

namespace Amon.PontoE.Servico.Cadastro
{
   public class FuncionarioServico : ICrudServico<Funcionario>
    {
        private readonly FuncionarioDAO dao;

        #region Construtores
        public FuncionarioServico()
        {
            dao = new FuncionarioDAO();
        }

        public FuncionarioServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<FuncionarioDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Funcionario obter(String id)
        {
            return dao.obter(new Funcionario { Id = id });
        }

        public Funcionario obter(int id)
        {
            return dao.obter(new Funcionario { Id = id.ToString() });
        }
        
        public IList<Funcionario> obterTodos()
        {
            return dao.obterTodos();
        }

        public Funcionario incluir(Funcionario entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Funcionario entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Funcionario entidade)
        {
            dao.excluir(entidade);
        }

        #endregion

        public IList<Funcionario> obterPorLotacao(String idLotacao)
        {
            return dao.obterPorLotacao(idLotacao);
        }

        public Funcionario obterPorLoginAD(String loginAd)
        {
            return dao.obterPorLoginAD(loginAd);
        }

        public IList<Funcionario> obterPorMatricula(String idMatricula)
        {
            return dao.obterPorMatricula(idMatricula);
        }

        public IList<Funcionario> obterDaMesmaLotacaoPorMatricula(String id, String lotacao)
        {
            return dao.obterDaMesmaLotacaoPorMatricula(id, lotacao);
        }

        public IList<Funcionario> verificaSeEGerente(String idFuncionario)
        {
            return dao.verificaSeEGerente(idFuncionario);
        }
    }
}
