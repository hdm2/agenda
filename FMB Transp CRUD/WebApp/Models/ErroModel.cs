using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amon.PontoE.WebApp.Models
{
    public class ErroModel
    {
        public String Titulo { get; set; }
        public String Mensagem { get; set; }

        public ErroModel(String titulo, String mensagem)
        {
            this.Titulo = titulo;
            this.Mensagem = mensagem;
        }
    }
}