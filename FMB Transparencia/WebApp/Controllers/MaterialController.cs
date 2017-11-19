using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Amon.PontoE.Modelo.Cadastro;
//using Amon.PontoE.Modelo.Material;
//using Amon.PontoE.WebApp.Models.Material;

namespace Amon.PontoE.WebApp.Controllers
{
    public class MaterialController : Controller
    {/*
        // GET: Material
        public ActionResult Index()
        {
            ModelTelaListagem modelo = new ModelTelaListagem();
            modelo.lista = new List<PlanoContas>();
            modelo.obj = new PlanoContas();
            modelo.obj.CampoBoolean = true;
            modelo.obj.CampoBoolean2 = true;

            List<ItemDropDown> itens = new List<ItemDropDown>();
            itens.Add(new ItemDropDown("Item 1", "1"));
            itens.Add(new ItemDropDown("Item 2", "2"));
            itens.Add(new ItemDropDown("Item 3", "3"));
            ViewBag.ItensDropDown = itens;

            for (int i = 0; i < 10; i++)
            {
                PlanoContas obj = new PlanoContas();
                obj.CampoAlfa = "Texto " + i + 1;
                obj.CampoDate = DateTime.Now.AddDays(i);
                obj.CampoNumerico = 12.67 * i;
                obj.CampoBoolean = i % 2 == 0 ? true : false;
                modelo.lista.Add(obj);
            }

            return View(modelo);
        }

        public ActionResult AcaoForm(ModelTelaListagem obj)
        {
            return RedirectToAction("Index");
        }   

        public ActionResult AcaoLinkVisualizar(int id)
        {
            return RedirectToAction("Index");
        }*/
    }
}