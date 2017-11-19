using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using System.Text;
using Amon.PontoE.Persistencia.Cadastro;
using Amon.Nucleo.Persistencia;

namespace Amon.PontoE.Servico.Cadastro
{
    public class LotacaoServico : ICrudServico<Lotacao>
    {
        private readonly LotacaoDAO dao;

        #region Construtores
        public LotacaoServico()
        {
            dao = new LotacaoDAO();
        }

        public LotacaoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<LotacaoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico
        public Lotacao obter(String id)
        {
            return dao.obter(new Lotacao { Id = id });
        }

       public Lotacao obter(int id)
       {
           return dao.obter(new Lotacao { Id = id.ToString() });
       }

        public IList<Lotacao> obterTodos()
        {
            return dao.obterTodos();
        }

        public Lotacao incluir(Lotacao entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(Lotacao entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(Lotacao entidade)
        {
            dao.excluir(entidade);
        }

        #endregion


        public void incluirNaoBloqueadas(String id)
        {
            dao.insereNaoBloqueada(id);
        }


        public void excluirNaoBloqueada(String id)
        {
            dao.deleteNaoBloqueada(id);
        }

        public IList<Lotacao> obterLotacoesNaoBloqueadas()
        {
            return dao.obterLotacoesNaoBloqueadas();
        }

        public Boolean verificaSeBloqueada(String id)
        {
            return dao.verificaSeBloqueada(id);
        }

        public IList<Lotacao> obterLotacoesPorFuncionario(String idFuncionario, DateTime data)
       {
           return dao.obterLotacoesPorFuncionario(idFuncionario, data);
       }

       public IList<Lotacao> obterTodosExcetoNaoBloqueadas()
       {
           return dao.obterTodosExcetoNaoBloqueadas();
       }

       public String obterLotacoesPorFuncionario(String idFuncionario)
       {
           IList<Lotacao> lista = dao.obterLotacoesPorFuncionario(idFuncionario, DateTime.Today);
           StringBuilder l = new StringBuilder("");
           foreach (Lotacao lotacao in lista)
           {
               l.AppendFormat("'{0}',", lotacao.Id);
           }
           return l.ToString().Substring(0, l.Length - 1);
       }

    }
}