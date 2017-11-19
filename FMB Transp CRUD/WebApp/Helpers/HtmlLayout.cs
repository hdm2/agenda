using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amon.PontoE.WebApp.Helpers
{
    public static class HtmlLayout
    {
        public static FormularioLayout iniciarLayout(this HtmlHelper htmlHelper)
        {
            FormularioLayout formulario = new FormularioLayout(htmlHelper.ViewContext);
            return formulario;
        }

        public static void finalizarLayout(this HtmlHelper htmlHelper)
        {
            HtmlLayout.finalizarLayout(htmlHelper.ViewContext);
        }

        internal static void finalizarLayout(ViewContext viewContext)
        {
            viewContext.Writer.WriteLine("</div>");
            viewContext.Writer.WriteLine("</div>");
            viewContext.Writer.WriteLine("</div>");
        }
    }
}