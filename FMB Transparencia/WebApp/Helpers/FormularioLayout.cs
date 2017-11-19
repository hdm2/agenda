using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Amon.PontoE.WebApp.Helpers
{
    public class FormularioLayout : IDisposable
    {
        private readonly ViewContext viewContext;
        private bool disposed;

        internal bool colorButton = true;
        internal bool editButton = true;
        internal bool toggleButton = true;
        internal bool deleteButton = true;
        internal bool fullscreenButton = true;
        internal bool customButton = true;
        internal bool collapsed = true;
        internal bool sortable = true;
        internal String titulo = "";
        internal String icone = "";
        internal String toolBar = "";
        internal String adicionarButton = "";
        internal String classes = "";

        internal String botaoAdicionarHtml = @"<div class='col-xs-3 col-sm-7 col-md-3 col-lg-2'>
												<a class='btn btn-success' href='{0}'>
													<i class='fa fa-plus'></i> <span class='hidden-mobile'>Adicionar</span>
												</a>
											</div>";

        internal IDictionary<string, object> htmlAttributes;

        public FormularioLayout(ViewContext vc)
        {
            if (vc == null)
                throw new ArgumentNullException("vc");
            this.viewContext = vc;
            this.htmlAttributes = new Dictionary<string, object>();
        }

        public void Dispose()
        {
            if (disposed)
                return;
            this.disposed = true;
            HtmlLayout.finalizarLayout(this.viewContext);
            GC.SuppressFinalize((object)this);
        }

        private void finalizarLayout()
        {
            Dispose();
        }

        #region MetodosPublicos
        public FormularioLayout botaoCor()
        {
            colorButton = false;
            return this;
        }
        public FormularioLayout botaoEditar()
        {
            editButton = false;
            return this;
        }
        public FormularioLayout botaoToggle()
        {
            toggleButton = false;
            return this;
        }
        public FormularioLayout botaoDelete()
        {
            deleteButton = false;
            return this;
        }
        public FormularioLayout botaoFullScren()
        {
            fullscreenButton = false;
            return this;
        }
        public FormularioLayout botaoCustom()
        {
            customButton = false;
            return this;
        }
        public FormularioLayout comecarFechado()
        {
            collapsed = false;
            return this;
        }
        public FormularioLayout podeMover()
        {
            sortable = false;
            return this;
        }
        public FormularioLayout setarTitulo(String tituloJanela)
        {
            titulo = tituloJanela;
            return this;
        }
        public FormularioLayout setarIcone(String iconeJanela)
        {
            icone = iconeJanela;
            return this;
        }
        public FormularioLayout setarAtributo(String chave, Object valor)
        {
            htmlAttributes.Add(chave, valor);
            return this;
        }
        public FormularioLayout setarClasses(String calssesJanela)
        {
            classes = calssesJanela;
            return this;
        }

        public FormularioLayout setarItemParaToolBar(String toolbarItens)
        {
            toolBar = toolbarItens;
            return this;
        }

        public FormularioLayout setarActionBotaoAdicionar(String action)
        {
            adicionarButton = action;
            return this;
        }

        public FormularioLayout desenhar(String id)
        {
            TagBuilder divPrincipal = new TagBuilder("div");
            divPrincipal.AddCssClass("jarviswidget");

            if (colorButton)
                divPrincipal.MergeAttribute("data-widget-colorbutton", "false");
            if (editButton)
                divPrincipal.MergeAttribute("data-widget-editbutton", "false");
            if(toggleButton)
                divPrincipal.MergeAttribute("data-widget-togglebutton", "false");
            if(deleteButton)
                divPrincipal.MergeAttribute("data-widget-deletebutton", "false");
            if(fullscreenButton)
                divPrincipal.MergeAttribute("data-widget-fullscreenbutton", "false");
            if(customButton)
                divPrincipal.MergeAttribute("data-widget-custombutton", "false");
            if(collapsed)
                divPrincipal.MergeAttribute("data-widget-collapsed", "false");
            if (sortable)
                divPrincipal.MergeAttribute("data-widget-sortable", "false");

            divPrincipal.MergeAttribute("id", id);

            divPrincipal.MergeAttributes(htmlAttributes, true);

            divPrincipal.AddCssClass(classes);

            viewContext.Writer.WriteLine(divPrincipal.ToString(TagRenderMode.StartTag));
            viewContext.Writer.WriteLine("<header>");
            if (!String.IsNullOrEmpty(icone))
                viewContext.Writer.WriteLine("<span class='widget-icon'> <i class='{0}'></i> </span>", icone);
			viewContext.Writer.WriteLine("<h2>{0}</h2>", titulo);
            viewContext.Writer.WriteLine("</header>");
            viewContext.Writer.WriteLine("<div>");
            viewContext.Writer.WriteLine("<div class='widget-body'>");
            if (!String.IsNullOrEmpty(toolBar) || !String.IsNullOrEmpty(adicionarButton))
            {
                 viewContext.Writer.WriteLine(@"<div class='widget-body-toolbar'>
											        <div class='row'>
												        {0}
                                                        {1}
											        </div>
										        </div>", String.IsNullOrEmpty(toolBar) ? "" : toolBar,
                                                      String.IsNullOrEmpty(adicionarButton) ? "" : String.Format(botaoAdicionarHtml, adicionarButton));
            }

            return this;
        }
        #endregion
    }
}