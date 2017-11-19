using Amon.Modelo.Entidades;
using Amon.Web.Models;
using Amon.Web.Models.ComponentesForm;
using Amon.Servico.PlanoContas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amon.Web.Controllers
{
    public class PlanoDeContasController : Controller
    {
        private const String actionIndex = "Index";
        private PlanoDeContasServico servico = new PlanoDeContasServico();

        // GET: PlanoDeContas
        public ActionResult Index()
        {
            ModelTelaListagem modelo = new ModelTelaListagem();
            modelo.lista = servico.obterTodos();

            #region  Construção do combobox
            //Estes itens podem vir de um enumerador ou da base
            List<ItemDropDown> itens = new List<ItemDropDown>();
            itens.Add(new ItemDropDown("Item 1", "1"));
            itens.Add(new ItemDropDown("Item 2", "2"));
            itens.Add(new ItemDropDown("Item 3", "3"));
            ViewBag.ItensDropDown = itens;
            #endregion

            return View(modelo);
        }

        [HttpPost]
        public ActionResult AcaoPrimaria(ModelTelaListagem modelo)
        {
            return RedirectToAction(actionIndex);
        }

        [HttpPost]
        public ActionResult AcaoSecundaria(ModelTelaListagem modelo)
        {
            return RedirectToAction(actionIndex);
        }

        public ActionResult LinkSimples()
        {
            return RedirectToAction(actionIndex);
        }


        //Ações dos itens de lista:
        [HttpPost]
        public ActionResult Excluir(String codCli, String codConta)
        {
            PlanoDeContas pc = servico.obterPorChaves(codCli, codConta);
            servico.excluir(pc);
            return RedirectToAction(actionIndex);
        }
    }
}
