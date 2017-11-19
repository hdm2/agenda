using System;
using System.Collections.Generic;
using Amon.Nucleo.Persistencia;
using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Ponto;
using Amon.PontoE.Persistencia.Ponto;

namespace Amon.PontoE.Servico.Ponto
{
    public class TipoAutorizacaoServico : ICrudServico<TipoAutorizacao>
    {
        private readonly TipoAutorizacaoDAO dao;

        #region Construtores
        public TipoAutorizacaoServico()
        {
            dao = new TipoAutorizacaoDAO();
        }

        public TipoAutorizacaoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<TipoAutorizacaoDAO>();
        }
        #endregion

        #region Implementação de ICrudServico

        public TipoAutorizacao obter(int id)
        {
            return dao.obter(new TipoAutorizacao {Id = id});
        }

        public IList<TipoAutorizacao> obterTodos()
        {
            return dao.obterTodos();
        }

        public TipoAutorizacao incluir(TipoAutorizacao entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(TipoAutorizacao entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(TipoAutorizacao entidade)
        {
            dao.excluir(entidade);
        }
        #endregion

        //Obtém os tipos conforme a regra de visibilidade da coluna "visivelPara"
        public IList<TipoAutorizacao> obterTiposVisiveis(String idLotacao)
        {
            return dao.obterTiposVisiveis(idLotacao);
        }

        public IList<TipoAutorizacao> obterTiposVigentes(IList<TipoAutorizacao> tipos)
        {
            return dao.filtrarTiposVigentes(tipos);
        }
    }
}