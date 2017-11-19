using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amon.PontoE.WebApp.Models
{
    public class ErroModel
    {
        public const String ErroTag = "ErroModel";
        public String Titulo { get; set; }
        public String Mensagem { get; set; }
    }
}