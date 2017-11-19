using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amon.Nucleo.Atributos;

namespace Amon.Nucleo.Utils
{
    public class MapeamentoUtils
    {
        
        public static Dictionary<String, String> obterMapeamento(Type tipo, String sufixoMapeamento = "")
        {
            Dictionary<string, string> mapeamento = new Dictionary<string, string>();
            //MapeamentoImplicito mi = tipo.GetCustomAttribute<MapeamentoImplicito>(true);
            //object[] attributes = tipo.GetCustomAttributes(typeof (MapeamentoImplicito), true);
            MapeamentoImplicito mi = tipo.obterAtributo<MapeamentoImplicito>(true); //attributes.FirstOrDefault() as MapeamentoImplicito;
            PropertyInfo[] propriedades = tipo.GetProperties();
            if (mi == null)
            {
                foreach (PropertyInfo p in propriedades)
                {
                    Mapear mapear = p.obterAtributo<Mapear>(true);
                    if (mapear == null)
                        continue;
                    mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco, sufixoMapeamento));
                }
            }
            else
            {
                foreach (PropertyInfo p in propriedades)
                {
                    NaoMapear naoMapear = p.obterAtributo<NaoMapear>(true);
                    if (naoMapear != null)
                        continue;

                    Mapear mapear = p.obterAtributo<Mapear>(true);
                    if (mapear == null)
                        mapeamento.Add(p.Name, converteMapeamento(p.Name, sufixoMapeamento));
                    else
                        mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco, sufixoMapeamento));
                }
            }
            return mapeamento;
        }

        public static Dictionary<String, String> obterMapeamento(object instancia, String sufixoMapeamento)
        {
            Type tipo = instancia.GetType();
            return obterMapeamento(tipo, sufixoMapeamento);
        }

        private static String converteMapeamento(String entrada, String sufixoMapeamento)
        {
            String saida = entrada;
            if (!String.IsNullOrEmpty(sufixoMapeamento))
            {
                saida = String.Format("{0}.{1}", sufixoMapeamento, entrada);
            }

            return saida;
        }
    }
}