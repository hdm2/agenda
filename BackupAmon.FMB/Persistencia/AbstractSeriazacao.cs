using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Amon.Nucleo.Atributos;
using Amon.Nucleo.Utils;

namespace Amon.Persistencia
{
    public abstract class AbstractSeriazacao
    {
        public static List<Resultado> obterLista<Resultado>(IDataReader dr, int maxLinhas = 0)
        {
            Type tipo = typeof (Resultado);
            
            ConstructorInfo construtor = tipo.GetConstructor(Type.EmptyTypes);
            if (construtor == null)
                throw new Exception(String.Format("Não foi possível encontrar construtor vazio para a classe {0}.", tipo.FullName));

            Dictionary<String, String> mapeamento = MapeamentoUtils.obterMapeamento(tipo);

            throw new Exception();
        }

        public static IList<MapeamentoInfo> obterMapeamento(Type tipo, String sufixoMapeamento = "")
        {
            IList<MapeamentoInfo> mapeamento = new List<MapeamentoInfo>();
            MapeamentoImplicito mi = tipo.obterAtributo<MapeamentoImplicito>(true);
            PropertyInfo[] propriedades = tipo.GetProperties();
            if (mi == null)
            {
                foreach (PropertyInfo p in propriedades)
                {
                    Mapear mapear = p.obterAtributo<Mapear>(true);
                    if (mapear == null)
                        continue;
                    //mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco, sufixoMapeamento));
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
                    //if (mapear == null)
                    //    mapeamento.Add(p.Name, converteMapeamento(p.Name, sufixoMapeamento));
                    //else
                    //    mapeamento.Add(p.Name, converteMapeamento(String.IsNullOrEmpty(mapear.ColunaBanco) ? p.Name : mapear.ColunaBanco, sufixoMapeamento));
                }
            }
            return mapeamento;
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

        public class MapeamentoInfo
        {
            public String Nome { get; set; }
            public String Correspondente { get; set; }
            public PropertyInfo Info { get; set; }
            public ConstructorInfo Construtor { get; set; }
            public bool UsarConstrutor { get { return Construtor != null; } }
            public IList<MapeamentoInfo> Mapeamentos { get; set; }
        }
    }
}