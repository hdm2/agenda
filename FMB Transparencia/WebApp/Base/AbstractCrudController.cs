using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using Amon.Nucleo.Entidade;
using Amon.Nucleo.Servico;
using Amon.Nucleo.Utils;
using Newtonsoft.Json;

namespace Amon.PontoE.WebApp.Base
{
    public abstract class AbstractCrudController<Tipo> : AbstractPaginadorController<Tipo> where Tipo : IEntidade, new()
    {
        protected ICrudServico<Tipo> servico;
        //protected readonly OperacaoServico operacaoServico;
        
        protected abstract String obterNomeOperacao();

        protected AbstractCrudController(ICrudServico<Tipo> s)
        {
            servico = s;
            //operacaoServico = new OperacaoServico();
        }

        protected AbstractCrudController()
        {
            //operacaoServico = new OperacaoServico();
        }

        public virtual ActionResult Manutencao(int id = 0)
        {
            return id == 0 ? View(new Tipo()) : View(servico.obterPorIndice(id));
        }

        [HttpPost]
        public virtual ActionResult Salvar(Tipo entidade)
        {
            bool inclusao = entidade["Id"].Equals(0);
            if (inclusao)
                entidade = servico.incluir(entidade);
            else
                servico.atualizar(entidade);

            String objStr = JsonConvert.SerializeObject(entidade);
            //Operacao operacao = operacaoServico.obterPorNome(obterNomeOperacao());
            //registraLog(String.Format("{0} > {1}", operacao.Comentario, inclusao ? "Inclusão de Registro" : "Edição de Registro"),objStr);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Excluir(int id, int pagina = 1)
        {
            try
            {
                Tipo e = ListaNaoPaginada.First(x => (int)x["Id"] == id);
                servico.excluir(e);
                ListaNaoPaginada.Remove(e);

                String objStr = JsonConvert.SerializeObject(e);
                //Operacao operacao = operacaoServico.obterPorNome(obterNomeOperacao());
                //registraLog(String.Format("{0} > Exclusão de Registro", operacao.Comentario), objStr);

                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [NonAction]
        protected void registraLog(String contexto, String operacao)
        {
            
        }
    }
}