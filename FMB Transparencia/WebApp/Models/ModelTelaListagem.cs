using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amon.Modelo.Entidades;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Amon.Web.Models
{
    public class ModelTelaListagem
    {
        //public PlanoDeContas obj { get; set; }
        public PlanoDeContas obj { get; set; }
        public IList<PlanoDeContas> lista { get; set; }

        public ModelTelaListagem()
        {
            //this.obj = new PlanoDeContas();
            this.obj = new PlanoDeContas();
            this.lista = new List<PlanoDeContas>();
        }
    }
}