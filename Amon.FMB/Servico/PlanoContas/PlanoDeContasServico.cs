using System;
using System.Collections.Generic;
using System.Linq;
using Amon.Nucleo.Servico;
using Amon.Modelo.Entidades;
using Amon.DAO.PlanoContas;
using Amon.Nucleo.Utils;

namespace Amon.Servico.PlanoContas
{
    public class PlanoDeContasServico : ICrudServico<PlanoDeContas>
    {
        private readonly PlanoDeContasDAO dao = new PlanoDeContasDAO();

        #region Implementações
        public PlanoDeContas obter(int id)
        {
            throw new NotImplementedException();
        }

        public PlanoDeContas obterPorChaves(int codCli, int codConta)
        {
            return dao.obterPorChaves(codCli, codConta).FirstOrDefault();
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


        //Busca o PlanoDeContas a partir do formulário de filtro
        public IList<PlanoDeContas> ObterPorFiltro(PlanoDeContas filtrado = null)
        {
            IEnumerable<PlanoDeContas> planos = new List<PlanoDeContas>();

            if (filtrado.CodCli != null && filtrado.CodContas != null) {
                int[] chaves = new int[] { filtrado.CodCli.Value, filtrado.CodContas.Value };
                return dao.obterPorChaves(chaves[0], chaves[1]);
            }

            if (filtrado.CodCli != null)
            {
                IList<PlanoDeContas> temp = dao.ObterPorCodigoCliente(filtrado.CodCli.Value);
                planos = !planos.Any() ? temp : planos.Intersect(temp);
            }

            if (filtrado.CodContas != null)
            {
                IList<PlanoDeContas> temp = dao.ObterPorCodigoConta(filtrado.CodContas.Value);
                planos = !planos.Any() ? temp : planos.Intersect(temp);
            }

            if (ApoioUtils.dataValida(filtrado.DataCriacao))
            {
                IList<PlanoDeContas> temp = dao.ObterPorDataCriacao(filtrado.DataCriacao);
                planos = !planos.Any() ? temp : planos.Intersect(temp);
            }

            if (ApoioUtils.dataValida(filtrado.DataExclusao))
            {
                IList<PlanoDeContas> temp = dao.ObterPorDataExclusao(filtrado.DataExclusao);
                planos = !planos.Any() ? temp : planos.Intersect(temp);
            }

            if (!String.IsNullOrEmpty(filtrado.Descricao))
            {
                IList<PlanoDeContas> temp = dao.ObterPorDescricao(filtrado.Descricao);
                planos = !planos.Any() ? temp : planos.Intersect(temp);
            }
            
            return planos.ToList();
        }
    }
}
