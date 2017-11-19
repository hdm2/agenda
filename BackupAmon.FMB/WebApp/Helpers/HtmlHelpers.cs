using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using Amon.PontoE.Modelo.Seguranca;

namespace Amon.PontoE.WebApp.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var builder = new TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }
        
        #region Mascara
        public static IHtmlString CampoMascaraPara<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, String mascara)
        {
            return CampoMascaraPara(htmlHelper, expression, mascara, null);
        }

        public static IHtmlString CampoMascaraPara<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, String mascara, object htmlAttributes)
        {
            return CampoMascaraPara(htmlHelper, expression, mascara, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString CampoMascaraPara<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, String mascara, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            String nome = metadata.PropertyName; //ExpressionHelper.GetExpressionText(expression);
            String idNome = String.Format("{0}M", nome);
            String idBind = String.Format("#{0}", nome);

            if (htmlAttributes == null)
            {
                htmlAttributes = new RouteValueDictionary();
            }

            RouteValueDictionary padrao = new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(new { data_mask = mascara, data_bind_semmask = idBind }));
            foreach (var atb in htmlAttributes)
            {
                padrao.Add(atb.Key, atb.Value);
            }


            MvcHtmlString txt = htmlHelper.TextBox(idNome, metadata.Model, padrao);
            MvcHtmlString hid = htmlHelper.HiddenFor(expression);
            StringBuilder sb = new StringBuilder(txt.ToHtmlString());
            sb.AppendLine(hid.ToHtmlString());
            return new MvcHtmlString(sb.ToString());
        }
        #endregion

        #region CampoValor

        #endregion

        #region Calendario
        public static IHtmlString CampoDataPara<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata.ModelType != typeof(DateTime) &&
                (metadata.ModelType.Name.IndexOf("Nullable", StringComparison.Ordinal) > -1 && Nullable.GetUnderlyingType(metadata.ModelType) != typeof(DateTime)))
            {
                throw new Exception(String.Format("Propriedade {0} não é do tipo DateTime.", metadata.PropertyName));
            }

            String nome = metadata.PropertyName; //ExpressionHelper.GetExpressionText(expression);
            MvcHtmlString txt = htmlHelper.TextBox(nome, metadata.Model, new { @class = "datepicker", data_mask = "00/00/0000", data_date_format = "dd/mm/yyyy", width = "" });

            StringBuilder sb = new StringBuilder(txt.ToHtmlString());

            TagBuilder icone = new TagBuilder("i");
            icone.AddCssClass("icon-append fa fa-calendar");

            TagBuilder r = new TagBuilder("div");
            r.AddCssClass("input-group");
            r.InnerHtml = icone + sb.ToString();

            return new MvcHtmlString(r.ToString());
        }
        #endregion

        public static IHtmlString RenderStylesIe(this HtmlHelper htmlHelper, string ie, params string[] paths)
        {
            var tag = string.Format(@"<!--[if {0}]>
{1}<![endif]-->", ie, Styles.Render(paths));
            return new MvcHtmlString(tag);
        }

        public static IHtmlString RenderScriptIe(this HtmlHelper htmlHelper, string ie, params string[] paths)
        {
            var tag = string.Format(@"<!--[if {0}]>
{1}<![endif]-->", ie, Scripts.Render(paths));
            return new MvcHtmlString(tag);
        }

        
    }
}