using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Amon.PontoE.WebApp.Helpers
{
    public enum ModoIcone
    {
        NaFrente, Atras
    }

    public enum ModoTooltip
    {
        AcimaEsquerda,
        AcimaDireita,
        AbaixoEsquerda,
        AbaixoDireita
    }

    public enum TipoItem
    {
        Texto,
        Inteiro,
        Decimal,
        Senha,
        Combo,
        ComboMulti,
        Check,
        CheckChave,
        AreaTexto
    }

    public class FormularioItem
    {
        protected String nome;
        protected Object valor;
        protected String icone;
        protected ModoIcone modoIcone;
        protected String tooltip;
        protected ModoTooltip modoTooltip;
        protected String labelTexto;
        protected TagBuilder section;
        protected TagBuilder label;
        protected TagBuilder labelInput;
        protected TagBuilder input;
        protected TagBuilder iconeTag;
        protected TagBuilder tooltipTag;
        protected TagBuilder erro;
        protected readonly TipoItem tipo;
        protected String textoMarcado = "Sim";
        protected String textoDesmarcao = "Não";
        protected String textoCheck;
        protected String valorMarcado = "true";
        protected String mascara;
        protected String format;
        protected IDictionary<String, Object> atrbutosExtras;

        protected readonly HtmlHelper htmlHelper;


        public static FormularioItem iniciar(TipoItem tipoCampo, HtmlHelper hh)
        {
            return new FormularioItem(tipoCampo, hh);
        }

        public FormularioItem(TipoItem tipoCampo, HtmlHelper hh)
        {
            nome = "teste";
            tipo = tipoCampo;
            htmlHelper = hh;
            section = new TagBuilder("section");
            label = new TagBuilder("label");
            labelInput = new TagBuilder("label");
            input = new TagBuilder("input");
            iconeTag = new TagBuilder("i");
            tooltipTag = new TagBuilder("b");
            erro = new TagBuilder("em");
            atrbutosExtras = new Dictionary<string, object>();
        }

        #region Metodos do contrustor
        public FormularioItem setarLabel(String textoLabel)
        {
            label.InnerHtml = textoLabel;
            label.AddCssClass("label");
            return this;
        }

        public FormularioItem setarIcone(String ico, ModoIcone modo = ModoIcone.Atras)
        {
            icone = ico;
            modoIcone = modo;
            return this;
        }

        public FormularioItem setarTooltip(String texto, ModoTooltip modo = ModoTooltip.AbaixoDireita)
        {
            tooltip = texto;
            modoTooltip = modo;
            return this;
        }

        public FormularioItem setarValor(Object vl, String valorCasoMarcado="true")
        {
            valor = vl;
            valorMarcado = valorCasoMarcado;
            return this;
        }

        public FormularioItem setarFormatacao(String strFormato)
        {
            format = strFormato;
            return this;
        }

        public FormularioItem setarMascara(String strMasc)
        {
            mascara = strMasc;
            return this;
        }

        public FormularioItem adicionarClassSection(String strClass)
        {
            section.AddCssClass(strClass);
            return this;
        }

        public FormularioItem adicionarAtributoSection(String nomeAtr, String valorAtr, bool sobrescrever = false)
        {
            section.MergeAttribute(nomeAtr, valorAtr, sobrescrever);
            return this;
        }

        public FormularioItem adicionarAtributoInput(String nomeAtr, String valorAtr)
        {
            atrbutosExtras.Add(nomeAtr, valorAtr);
            return this;
        }

        #endregion

        public IHtmlString desenhar()
        {
            
            switch (tipo)
            {
                case TipoItem.CheckChave:
                    labelInput.AddCssClass("toggle");
                    break;
                case TipoItem.Check:
                    labelInput.AddCssClass("checkbox");
                    break;
                case TipoItem.Combo:
                case TipoItem.ComboMulti:
                    labelInput.AddCssClass("select");
                    break;
                case TipoItem.AreaTexto:
                    labelInput.AddCssClass("textarea");
                    break;
                default:
                    labelInput.AddCssClass("input");
                    break;
            }
            //Inicia o SB com a tag Section
            StringBuilder sb = new StringBuilder(section.ToString(TagRenderMode.StartTag));

            //Testa se existe label e inclui na SB, caso exista
            if (!String.IsNullOrEmpty(label.InnerHtml))
                sb.AppendLine(label.ToString(TagRenderMode.Normal));

            //Inclui no SB a abertura da tag LABEL INPUT
            sb.AppendLine(labelInput.ToString(TagRenderMode.StartTag));

            //Verifica se esta sendo usado o icone e inclui no SB
            if (!String.IsNullOrEmpty(icone))
            {
                String iconeMode = modoIcone == ModoIcone.Atras ? "icon-append" : "icon-prepend";
                iconeTag.AddCssClass(iconeMode);
                iconeTag.AddCssClass(icone);
                sb.AppendLine(iconeTag.ToString(TagRenderMode.Normal));
            }


            String valorStr = htmlHelper.FormatValue(valor, format);

            switch (tipo)
            {
                case TipoItem.Check:
                case TipoItem.CheckChave:
                    input.MergeAttribute("type", "checkbox");
                    input.MergeAttribute("value", valorMarcado);
                    if (string.Equals(valorStr, valorMarcado, StringComparison.Ordinal))
                        input.MergeAttribute("checked", "checked");
                    input.MergeAttribute("id", nome);
                    input.MergeAttribute("name", nome);
                    sb.AppendLine(input.ToString(TagRenderMode.SelfClosing));
                    if (tipo == TipoItem.CheckChave)
                        sb.AppendFormat("<i data-swchon-text='{0}' data-swchoff-text='{1}'></i>{2}", textoMarcado, textoDesmarcao, textoCheck);
                    else
                        sb.AppendFormat("<i></i>{0}", textoCheck);
                    sb.AppendLine();
                    break;

                default:
                    if (String.IsNullOrEmpty(mascara))
                    {
                        String idBind = String.Format("#{0}", nome);
                        atrbutosExtras.Add("data-mask",mascara);
                        atrbutosExtras.Add("data-bind-semmask",idBind);
                        sb.AppendLine(htmlHelper.Hidden(idBind, valor).ToHtmlString());
                    }
                    sb.AppendLine(htmlHelper.TextBox(nome, valor, format, atrbutosExtras).ToHtmlString());
                    break;
            }


            

            if (!String.IsNullOrEmpty(tooltip))
            {
                String ttMode = "";
                switch (modoTooltip)
                {
                    case ModoTooltip.AbaixoDireita:
                        ttMode = "tooltip-bottom-right";
                        break;
                    case ModoTooltip.AbaixoEsquerda:
                        ttMode = "tooltip-bottom-left";
                        break;
                    case ModoTooltip.AcimaDireita:
                        ttMode = "tooltip-top-right";
                        break;
                    case ModoTooltip.AcimaEsquerda:
                        ttMode = "tooltip-top-left";
                        break;
                }
                tooltipTag.InnerHtml = tooltip;
                tooltipTag.AddCssClass("tooltip");
                tooltipTag.AddCssClass(ttMode);
                sb.AppendLine(tooltipTag.ToString(TagRenderMode.Normal));
            }

            sb.AppendLine(labelInput.ToString(TagRenderMode.EndTag));

            erro.MergeAttribute("for", nome);
            erro.AddCssClass("invalid");
            sb.AppendLine(erro.ToString(TagRenderMode.SelfClosing));


            sb.AppendLine(section.ToString(TagRenderMode.EndTag));


            return new MvcHtmlString(sb.ToString());
        }
    }
}