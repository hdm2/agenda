using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Ponto;
using System;
using System.Collections.Generic;
using Amon.PontoE.Persistencia.Ponto;
using Amon.Nucleo.Persistencia;

namespace Amon.PontoE.Servico.Ponto
{
   public class AutorizacaoServico : ICrudServico<Autorizacao>
    {
        private readonly AutorizacaoDAO dao;

        #region Construtores
        public AutorizacaoServico()
        {
            dao = new AutorizacaoDAO();
        }

        public AutorizacaoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<AutorizacaoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Autorizacao obter(int id)
        {
            return dao.obter(new Autorizacao { Id = id });
        }

        public IList<Autorizacao> obterTodos()
        {
            return dao.obterTodos();
        }

        public Autorizacao incluir(Autorizacao entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Autorizacao entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Autorizacao entidade)
        {
            dao.excluir(entidade);
        }

        #endregion

        public IList<Autorizacao> obterTodasDoFuncionario(String idFuncionario)
        {
            return dao.obterTodasDoFuncionario(idFuncionario);
        }

        public IList<Autorizacao> obterTodasDoDiaPorFuncionario(String idFuncionario)
        {
            return dao.obterTodasDoDiaPorFuncionario(idFuncionario);
        }

        public IList<Autorizacao> ObterAutorizacoesDoDiaNaoNotificadas(String idFuncionario)
        {
            return dao.ObterAutorizacoesDoDiaNaoNotificadas(idFuncionario);
        }

        public IList<Autorizacao> marcarComoNotificada(int idAutorizacao)
        {
            return dao.marcarComoNotificada(idAutorizacao);
        }

        public IList<Autorizacao> obterTodasPorData(String id, DateTime inicio, DateTime fim)
        {
            return dao.obterTodasPorData(id, inicio, fim);
        }

        public IList<Autorizacao> obterTodasAPartirDaData(String id, DateTime data)
        {
            return dao.obterTodasAPartirDaData(id, data);
        }
   }
}
