using Amon.Modelo.Entidades;
using Amon.Web.Models;
using Amon.Web.Models.ComponentesForm;
using Amon.Servico.PlanoContas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amon.Web.Utils;
using Amon.Nucleo.Utils;

namespace Amon.Web.Controllers
{
    public class PlanoDeContasController : Controller
    {
        private const String actionConsulta = "Index",
                             actionCadastro = "Cadastro";
        private PlanoDeContasServico servico = new PlanoDeContasServico();

        // GET: PlanoDeContas
        public ActionResult Index(int? status = null)
        {
            PlanoDeContas objPesquisado = new PlanoDeContas();
            ModelTelaListagem modelo = new ModelTelaListagem();
            if (TempData.ContainsKey("Filtro"))
            {
                if (TempData["Filtro"].GetType().IsEquivalentTo(typeof(PlanoDeContas)))
                {
                    modelo.lista = servico.ObterPorFiltro(TempData["Filtro"] as PlanoDeContas);
                    TempData.Remove("filtro");
                    if (!modelo.lista.Any())
                        status = Mensagens.NotificacaoNenhumItemEncontrado;
                }
                else if (TempData["Filtro"] == "TODOS")
                    modelo.lista = servico.obterTodos();
                else
                    status = Mensagens.NotificacaoErro;
            }

            #region  Construção do combobox
            //Estes itens podem vir de um enumerador ou da base
            List<ItemDropDown> itens = new List<ItemDropDown>();
            itens.Add(new ItemDropDown("Item 1", "1"));
            itens.Add(new ItemDropDown("Item 2", "2"));
            itens.Add(new ItemDropDown("Item 3", "3"));
            ViewBag.ItensDropDown = itens;
            #endregion

            if (status != null)
                ViewBag.InfoMsg = Mensagens.GetStatusMessage(status);

            return View(modelo);
        }

        //Ações do formulário:
        //Pesquisa
        [HttpPost]
        public ActionResult ResultadoPesquisa(ModelTelaListagem modelo)
        {
            TempData["Filtro"] = modelo.obj;
            return RedirectToAction(actionConsulta);
        }

        [HttpPost]
        public ActionResult ObterTodos()
        {
            TempData["Filtro"] = "TODOS";
            return RedirectToAction(actionConsulta);
        }

        [HttpPost]
        public ActionResult AcaoSecundaria(ModelTelaListagem modelo)
        {
            return RedirectToAction(actionConsulta);
        }

        public ActionResult LinkSimples()
        {
            return RedirectToAction(actionConsulta);
        }


        //Ações dos itens de lista:
        [HttpPost]
        public ActionResult Excluir(String cli, String con)
        {
            int feedback = Mensagens.NotificacaoErro;

            try
            {
                PlanoDeContas pc = servico.obterPorChaves(cli, con);
                servico.excluir(pc);
                feedback = Mensagens.NotificacaoSucesso;
            }
            catch (Exception)
            {
                //Não é necessário implementar
            }

            return RedirectToAction(actionConsulta, new { status = feedback });
        }

        #region PENDENTE
        [HttpPost]
        public JsonResult CarregaDadosEdicao(String cli, String con)
        {
            PlanoDeContas plano = new PlanoDeContasServico().obterPorChaves(cli, con);
            return Json(new { planoEmEdicao = plano }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizarPlanoDeContas(PlanoDeContas dependente)
        {
            //try
            //{
            //    new DependenteServico().atualizar(dependente);
            //}
            //catch (Exception e)
            //{
            //    TempData["ErroMsg"] = Mensagens.ErroCadastroDependente;
            //}

            return RedirectToAction(actionConsulta);
        }
        #endregion

        //Abre o formulário de cadastro
        public ActionResult Cadastro()
        {
            PlanoDeContas plano = new PlanoDeContas();
            return View(plano);
        }

        //Faz a persistência na base
        [HttpPost]
        public ActionResult Cadastrar(PlanoDeContas plano)
        {
            int feedback = Mensagens.NotificacaoErro;

            if (servico.obterPorChaves(plano.CodCli.Value, plano.CodContas.Value) != null)
            {
                //Verificar primeiramente a existência do cliente e da conta
            }
            else if (false)
            {
                //esse IF deve verificar a tabela de clientes e identificar se o código inserido existe. O mesmo deve ser feito com o código da conta.
            }
            else
            {
                plano.DataCriacao = DateTime.Now;

                try
                {
                    servico.incluir(plano);
                    feedback = Mensagens.NotificacaoSucesso;
                    TempData["Filtro"] = plano;
                }
                catch (Exception e)
                {
                    //Não é necessário implementar
                }
            }

            return RedirectToAction(actionConsulta, new { status = feedback });
        }
    }
}
