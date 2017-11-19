using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amon.Web.Models.Formularios;
using Amon.Web.Models.Listagens;
using Amon.Modelo.Entidades;

namespace Amon.Web.Models
{
    public class ModelConsultaPlanoDeContas
    {
        public ModelFiltroPlanoDeContas filtro { get; set; }
        public IList<ModelListaPlanoDeConta> lista { get; set; }

        public ModelConsultaPlanoDeContas()
        {
            this.filtro = new ModelFiltroPlanoDeContas();
            this.lista = new List<ModelListaPlanoDeConta>();
        }
    }
}