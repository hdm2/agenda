using System;

namespace Amon.Web.Utils
{
    public class REGEX
    {
        //REFERÊNCIA: http://turing.com.br/material/regex/introducao.html

        public static String data = @"^[ ]|(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2)\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";

        public static String textoLivre = @".*";
        public static String numerico = @"[0-9]*";
        public static String alfa = @"[a-zA-Z]*";
        public static String alfanumericoComEspacos = @"[a-zA-Z0-9 ]*";
        public static String alfaComEspaco = @"[a-zA-Z ]*";

        public static String numericoQuantidadeDefinida(int min = 1, int max = 0) {
            String regex = @"[0-9]{" + min;
            if (max > min)
                regex += "," + max;
            regex += "}";
            return regex;
        }

        public static String alfaQuantidadeDefinida(int min = 1, int max = 0)
        {
            String regex = @"[a-zA-Z]{" + min;
            if (max > min)
                regex += "," + max;
            regex += "}";
            return regex;
        }

        public static String textoLivreQuantidadeDefinida(int min = 1, int max = 0)
        {
            String regex = @".{" + min;
            if (max > min)
                regex += "," + max;
            regex += "}";
            return regex;
        }

        public static String alfaComEspacosQuantidadeDefinida(int min = 1, int max = 0)
        {
            String regex = @"[a-zA-Z ]{" + min;
            if (max > min)
                regex += "," + max;
            regex += "}";
            return regex;
        }


        //Validadores específicos
        public static String nomeProprio = @"[A-Z][a-záéíóúàâêôãõüçÁÉÍÓÚÀÂÊÔÃÕÜÇ\-' ]+";
        public static String CEP = @"[0-9]{5}-[0-9]{3}";
        public static String telefone = @"\([1-9]{2}\) [0-9]{4}\-[0-9]{4}";
        public static String celular = @"\([1-9]{2}\) 9[0-9]{4}\-[0-9]{4}";
        public static String monetario = @"[0-9]{1,8}\.[0-9]{2}";
    }
}