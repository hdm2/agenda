using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Amon.PontoE.WebApp.Base
{
    public abstract class AbstractPaginadorController<Tipo> : Controller
    {
        protected IList<Tipo> ListaNaoPaginada
        {
            get { return Session[GetType().FullName] as IList<Tipo>; }
            set { Session[GetType().FullName] = value; }
        }

        protected IPagedList<Tipo> converterPaginacao(int? pagina, int tamanhoPagina = 20)
        {
            if (pagina.HasValue && pagina < 1)
                return null;

            if (ListaNaoPaginada == null)
                return null;

            decimal pagMax = (decimal)ListaNaoPaginada.Count / tamanhoPagina;
            if (pagMax - (int)pagMax != 0)
                pagMax = pagMax + (1 - (pagMax % 1));

            if (pagina > pagMax)
                pagina = (int)pagMax;
            if (pagina < 1)
                pagina = 1;

            IPagedList<Tipo> listaPaginada = ListaNaoPaginada.ToPagedList(pagina ?? 1, tamanhoPagina);

            if (listaPaginada.PageNumber != 1 && pagina.HasValue && pagina > listaPaginada.PageCount)
                return null;

            return listaPaginada;
        }

        public virtual ActionResult obterPagina(int? pagina, int tamanhoPagina = 20)
        {
            IPagedList<Tipo> listaPaginada = converterPaginacao(pagina, tamanhoPagina);
            if (listaPaginada == null)
                return HttpNotFound();

            PartialViewResult r = PartialView("_ListaGrid", listaPaginada);

            return r;
        }
	}
}