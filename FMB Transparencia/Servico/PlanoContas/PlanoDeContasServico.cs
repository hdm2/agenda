using System;
using System.Collections.Generic;
using System.Linq;
using Amon.Nucleo.Servico;
using Amon.Modelo.Entidades;
using Amon.DAO.PlanoContas;

namespace Amon.Servico.PlanoContas
{
    public class PlanoDeContasServico : ICrudServico<PlanoDeContas>
    {
        #region Implementações
        private readonly PlanoDeContasDAO dao = new PlanoDeContasDAO();

        public PlanoDeContas obterPorIndice(int id)
        {
            throw new NotImplementedException();
        }

        public PlanoDeContas obterPorChaves(String codCli, String codConta)
        {
            return dao.obterPorChaves(int.Parse(codCli), int.Parse(codConta)).FirstOrDefault();
        }

        public IList<PlanoDeContas> obterTodos()
        {
            return dao.obterTodos();
        }

        public void atualizar(PlanoDeContas entidadeAtualizada)
        {
            dao.atualizar(entidadeAtualizada);
        }

        public void excluir(PlanoDeContas entidade)
        {
            dao.excluir(entidade);
        }

        public PlanoDeContas incluir(PlanoDeContas entidade)
        {
            return dao.incluir(entidade);
        }
        #endregion

        //Código de negócio
        //       V

        public IList<PlanoDeContas> ObterPorDataCriacao(DateTime dataCriacao)
        {
            //Executa algum tratamento no parâmetro de entrada, se necessário

            IList<PlanoDeContas> planosDeConta = dao.ObterPorDataCriacao(dataCriacao);

            //Executa algum tratamento na lista 'planosDeConta', se necessário

            return planosDeConta;
        }

    }
}
