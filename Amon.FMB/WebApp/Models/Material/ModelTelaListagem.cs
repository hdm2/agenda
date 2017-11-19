using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amon.PontoE.Modelo.Material;

namespace Amon.PontoE.WebApp.Models.Material
{
    public class ModelTelaListagem
    {
        public ExemploObj obj { get; set; }
        public List<ExemploObj> lista { get; set; }
    }
}