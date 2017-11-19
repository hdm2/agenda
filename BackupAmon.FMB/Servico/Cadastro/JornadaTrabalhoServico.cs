using Amon.Nucleo.Servico;
using Amon.PontoE.Modelo.Cadastro;
using System;
using System.Collections.Generic;
using Amon.Nucleo.Persistencia;
using Amon.PontoE.Persistencia.Cadastro;

namespace Amon.PontoE.Servico.Cadastro
{
    public class JornadaTrabalhoServico : ICrudServico<JornadaTrabalho>
    {
        private readonly JornadaTrabalhoDAO dao;

        #region Construtores
         
        public JornadaTrabalhoServico()
        {
            dao = new JornadaTrabalhoDAO();
        }

        public JornadaTrabalhoServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<JornadaTrabalhoDAO>();
        }

        #endregion

        #region Implementação de ICrudServico
        public JornadaTrabalho obter(int id)
        {
            return obter(id.ToString());
        }

        public JornadaTrabalho obter(String id)
        {
            return dao.obter(new JornadaTrabalho {Id = id});
        }

        public IList<JornadaTrabalho> obterTodos()
        {
            return dao.obterTodos();
        }

        public JornadaTrabalho incluir(JornadaTrabalho entidade)
        {
            return dao.incluir(entidade);
        }

        public void atualizar(JornadaTrabalho entidade)
        {
            dao.atualizar(entidade);
        }

        public void excluir(JornadaTrabalho entidade)
        {
            dao.excluir(entidade);
        }

        #endregion

        public IList<JornadaTrabalho> obterNormalEAlternativas(String idFuncionario)
        {
            return dao.obterNormalEAlternativas(idFuncionario);
        }

        public JornadaTrabalho IdentificarJornadaTrabalho(String id, String horario)
        {
            return new JornadaTrabalhoDAO().IdentificarJornadaTrabalho(id, horario);
        }
    }
}