using System;
using Amon.Modelo.Entidades;
using System.Reflection;
using System.Collections.Generic;

namespace Teste
{
    public class Program
    {
        public static void Main2(string[] args)
        {
            PlanoDeContas plano = new PlanoDeContas();
            plano.Analitica = true;
            //plano.CodCli = 123;
            plano.CodContas = 111;
            plano.ContaReduz = 11;
            plano.DataCriacao = DateTime.Today;
            plano.DataExclusao = DateTime.Today;
            plano.Descricao = "Teste";
            plano.ValorOriginal = new Decimal(1455.44);

            try
            {
                PropertyInfo[] properties = plano.GetType().GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    Console.Write("Tipo: " + prop.GetValue(plano, null).GetType().Name + "    Valor: " + prop.GetValue(plano, null) + "    Nome: " + prop.Name + "       setado = ");

                    List<Type> tipos = new List<Type>() { typeof(Int32), typeof(Decimal), typeof(Double), typeof(DateTime), typeof(String) };

                    Type tipo = prop.GetValue(plano, null).GetType();
                    object valor = prop.GetValue(plano, null);

                    if (tipo.IsEquivalentTo(typeof(Int32)))
                    {
                        Console.WriteLine((Int32)valor != 0);
                    }
                    else if (tipo.IsEquivalentTo(typeof(Decimal)))
                    {
                        Console.WriteLine(!((Decimal)valor).Equals(0.0));
                    }
                    else if (tipo.IsEquivalentTo(typeof(Double)))
                    {
                        Console.WriteLine(!((Double)valor).Equals(0.0));
                    }
                    else if (tipo.IsEquivalentTo(typeof(DateTime)))
                    {
                        Console.WriteLine(((DateTime)valor).Year != 0001);
                    }
                    else if (tipo.IsEquivalentTo(typeof(String)))
                    {
                        Console.WriteLine(!String.IsNullOrEmpty((String)valor));
                    }
                    else
                    {
                        Console.WriteLine(valor != null);
                    }
                }
            }
            catch (TargetParameterCountException)
            {

            }

            Console.WriteLine("============= Concluído");
            Console.ReadKey(); Console.ReadKey();
        }

        public static void Main(string[] args)
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("1", "Mensagem 1");
            dic.Add("2", "Mensagem 2");
            Console.WriteLine(dic["1"]);
            Console.ReadKey();
        }
    }
}
