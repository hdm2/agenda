using System.Web.Optimization;

namespace Amon.PontoE.WebApp
{
    public class BundleConfig
    {
        // Para obter mais informações sobre agrupamento, visite http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender com ela. Após isso, quando você estiver
        //    // pronto para produção, use a ferramenta de compilação em http://modernizr.com para selecionar somente os testes que você precisa.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));
        //}

        public static void RegisterBundles(BundleCollection bundles)
        {
            ConfigureIgnoreList(bundles.IgnoreList);

            RegisterStyleBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }

        public static void ConfigureIgnoreList(IgnoreList ignoreList)
        {
            ignoreList.Clear();

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/cssbasic")
                            .Include("~/Content/css/bootstrap.min.css")
                            .Include("~/Content/css/font-awesome.min.css")
                            .Include("~/Content/css/smartadmin-production.css")
                            .Include("~/Content/css/smartadmin-skins.css")
                            .Include("~/Content/css/adicional.css")
                            .Include("~/Content/PagedList.css")
                            .Include("~/Content/css/datepicker.css")
                            .Include("~/Content/css/select2.css")
                            .Include("~/Content/css/radio-btn/bootstrap-switch.min.css")
                            );

            bundles.Add(new StyleBundle("~/cssie")
                           .Include("~/Content/css/ie7.css")
                           );
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/basico")
                            .Include("~/Scripts/js/libs/jquery-1.11.1.min.js")
                            .Include("~/Scripts/js/libs/jquery-ui-1.10.3.min.js")
                            .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
                            .Include("~/Scripts/js/bootstrap/bootstrap.min.js")
                            .Include("~/Scripts/js/plugin/msie-fix/jquery.mb.browser.min.js")
                            .Include("~/Scripts/js/plugin/fastclick/fastclick.js")
                            .Include("~/Scripts/js/notification/SmartNotification.js")
                            .Include("~/Scripts/js/smartwidgets/jarvis.widget.min.js")
                            .Include("~/Scripts/js/app.js")
                            );

            bundles.Add(new ScriptBundle("~/js/adicional")
                            .Include("~/Scripts/adicional.js")
                            .Include("~/Scripts/radio-btn/bootstrap-switch.min.js")
                            );

            bundles.Add(new ScriptBundle("~/js/charts")
                .Include("~/Scripts/js/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js")
                .Include("~/Scripts/js/plugin/sparkline/jquery.sparkline.min.js")
                .Include("~/Scripts/jquery-msgbox/jquery.msgbox.js")
                .Include("~/Scripts/Template/ace-extra.js")
                );

            bundles.Add(new ScriptBundle("~/js/flotCharts")
                .Include("~/Scripts/js/plugin/flot/jquery.flot.cust.js")
                .Include("~/Scripts/js/plugin/flot/jquery.flot.resize.js")
                .Include("~/Scripts/js/plugin/flot/jquery.flot.tooltip.js")
                .Include("~/Scripts/js/plugin/flot/jquery.flot.categories.js")
                .Include("~/Scripts/js/plugin/flot/jquery.flot.time.js")
                );

            bundles.Add(new ScriptBundle("~/js/vetorMaps")
                .Include("~/Scripts/js/plugin/vectormap/jquery-jvectormap-1.2.2.min.js")
                .Include("~/Scripts/js/plugin/vectormap/jquery-jvectormap-world-mill-en.js"));

            bundles.Add(new ScriptBundle("~/js/calendar")
                .Include("~/Scripts/js/plugin/fullcalendar/jquery.fullcalendar.min.js")
                );

            bundles.Add(new ScriptBundle("~/js/progressbar")
                .Include("~/Scripts/js/plugin/bootstrap-progressbar/bootstrap-progressbar.js"));
            
            bundles.Add(new ScriptBundle("~/js/galeria")
                .Include("~/Scripts/js/plugin/superbox/superbox.min.js"));

             bundles.Add(new ScriptBundle("~/js/dataTable")
                .Include("~/Scripts/js/plugin/datatables/jquery.dataTables-cust.min.js")
                .Include("~/Scripts/js/plugin/datatables/ColReorder.min.js")
                .Include("~/Scripts/js/plugin/datatables/FixedColumns.min.js")
                .Include("~/Scripts/js/plugin/datatables/ColVis.min.js")
                .Include("~/Scripts/js/plugin/datatables/ZeroClipboard.js")
                .Include("~/Scripts/js/plugin/datatables/media/js/TableTools.min.js")
                .Include("~/Scripts/js/plugin/datatables/DT_bootstrap.js")
                );

            bundles.Add(new ScriptBundle("~/js/forms")
                .Include("~/Scripts/js/plugin/jquery-form/jquery-form.min.js")
                .Include("~/Scripts/js/plugin/jquery-validate/jquery.validate.min.js")
                .Include("~/Scripts/jquery.mask.min.js")
                .Include("~/Scripts/js/plugin/maxlength/bootstrap-maxlength.min.js")
                .Include("~/Scripts/js/plugin/bootstrap-datepicker.js")
                .Include("~/Scripts/js/plugin/bootstrap-datepicker.pt-BR.js")
                .Include("~/Scripts/customForm.js")
                .Include("~/Scripts/masker.js")
                .Include("~/Scripts/validator.js")
                );

            bundles.Add(new ScriptBundle("~/js/extraForms")
                .Include("~/Scripts/js/plugin/bootstrap-timepicker/bootstrap-timepicker.min.js")
                .Include("~/Scripts/js/plugin/bootstrap-tags/bootstrap-tagsinput.min.js")
                .Include("~/Scripts/js/plugin/colorpicker/bootstrap-colorpicker.min.js")
                .Include("~/Scriptsjs/plugin/knob/jquery.knob.min.js")
                .Include("~/Scripts/js/plugin/select2/select2.min.js")
                );

            bundles.Add(new ScriptBundle("~/js/nestable")
                .Include("~/Scripts/js/plugin/jquery-nestable/jquery.nestable.js"));

            bundles.Add(new ScriptBundle("~/js/sliders")
                .Include("~/Scripts/js/plugin/bootstrap-slider/bootstrap-slider.min.js")
                .Include("~/Scripts/js/plugin/noUiSlider/jquery.nouislider.min.js")
                .Include("~/Scripts/js/plugin/ion-slider/ion.rangeSlider.min.js"));

            bundles.Add(new ScriptBundle("~/js/x-editable")
                .Include("~/Scripts/js/plugin/x-editable/x-editable.min.js"));

            bundles.Add(new ScriptBundle("~/js/typehead")
                .Include("~/Scripts/js/plugin/typeahead/typeahead.min.js")
                .Include("~/Scripts/js/plugin/typeahead/typeaheadjs.min.js"));

            bundles.Add(new StyleBundle("~/js/ie")
                           .Include("~/Scripts/html5.js")
                           .Include("~/Scripts/respond.js")
                           );
        }
    }
}
