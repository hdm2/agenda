using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Amon.PontoE.Modelo.Seguranca;
using Amon.PontoE.WebApp.Models;

namespace Amon.PontoE.WebApp.Attributes
{
    public class AutenticarAttribute : ActionFilterAttribute
    {
        private readonly RouteValueDictionary acaoLogin = new RouteValueDictionary(new { Controller = "Autenticacao", Action = "Index" });
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                return;


            Usuario u = filterContext.HttpContext.Session["usuario"] as Usuario;
            if (filterContext.HttpContext.Session != null && u == null)
            {
                filterContext.HttpContext.Session["controller"] = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                filterContext.HttpContext.Session["action"] = filterContext.ActionDescriptor.ActionName;
                filterContext.HttpContext.Session["param"] = filterContext.RouteData;
                
                filterContext.Result = new RedirectToRouteResult(acaoLogin);
                return;
            }

            ErroModel acessoNegado = new ErroModel { Titulo = "ACESSO NEGADO", Mensagem = "Você não possui permissão para executar esta ação." };
            String controlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            String acao = filterContext.ActionDescriptor.ActionName;
            Object o = 0;
            bool inclusao = !filterContext.RouteData.Values.ContainsKey("id") ||
                            filterContext.RouteData.Values["id"] == o;
            bool semPermissao = u.Permissoes.Any(a => a.Controller == controlador && !a.Inclusao);
            if (acao == "Manutencao" && inclusao && semPermissao)
            {
                filterContext.Controller.TempData[ErroModel.ErroTag] = acessoNegado;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = controlador, Action = "Index" }));
                return;
            }

            semPermissao = u.Permissoes.Any(a => a.Controller == controlador && !a.Alteracao);
            if (acao == "Manutencao" && !inclusao && semPermissao)
            {
                filterContext.Controller.TempData[ErroModel.ErroTag] = acessoNegado;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = controlador, Action = "Index" }));
                return;
            }

            semPermissao = u.Permissoes.Any(a => a.Controller == controlador && !a.Exclusao);
            if (acao == "Excluir" && semPermissao)
            {
                ContentResult r = new ContentResult {Content = acessoNegado.Mensagem};
                filterContext.Result = r;
            }
        }
    }
}