using System;

namespace Amon.PontoE.Modelo.Material
{
    public class ItemDropDown
    {
        public String Texto { get; set; }
        public String Valor { get; set; }

        public ItemDropDown(String Texto, String Valor)
        {
            this.Texto = Texto;
            this.Valor = Valor;
        }
    }
}
