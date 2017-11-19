using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace Amon.Nucleo.Utils
{
    public static class ApoioUtils
    {
        public static String removeAcentos(String str)
        {
            string C_acentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇçºª&";
            string S_acentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCcoae";

            for (int i = 0; i < C_acentos.Length; i++)
                str = str.Replace(C_acentos[i].ToString(), S_acentos[i].ToString()).Trim();
            return str;
        }

        public static void converteParaClasse(Hashtable reg, Object classe)
        {
            Type tipo = classe.GetType();
            PropertyInfo[] propriedades = tipo.GetProperties();

            foreach (PropertyInfo p in propriedades)
            {
                if (!p.CanWrite || (reg[p.Name] == null))
                    continue;
                if ((p.PropertyType.Equals(typeof(int))) & (!reg[p.Name].GetType().Equals(typeof(int))))
                {
                    p.SetValue(classe, int.Parse(reg[p.Name].ToString()), null);
                }
                else
                {
                    p.SetValue(classe, reg[p.Name], null);
                }
            }
        }

        public static Dictionary<String, DbParameter> concatenaParametros(Dictionary<String, DbParameter> paramLista1, Dictionary<String, DbParameter> paramLista2)
        {
            Dictionary<String, DbParameter> orig;
            Dictionary<String, DbParameter> dest;
            if (paramLista1.Count > paramLista2.Count)
            {
                orig = paramLista1;
                dest = paramLista2;
            }
            else
            {
                orig = paramLista2;
                dest = paramLista1;
            }

            foreach (KeyValuePair<String, DbParameter> p in dest)
            {
                if (!orig.ContainsKey(p.Key))
                    orig.Add(p.Key, p.Value);
            }

            return orig;
        }

        public static int getCodConfig(String Tipo)
        {
            return ConverteUtils.sempreConverteInt32(ConfigurationManager.AppSettings[Tipo]);
        }

        public static String getStrConfig(String Tipo)
        {
            return ConfigurationManager.AppSettings[Tipo].ToString();
        }

        public static void limparMemoria()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static DateTime retornaUltimoDiaMes(int ano, int mes)
        {
            int ultimoDia = DateTime.DaysInMonth(ano, mes);
            DateTime data = new DateTime(ano, mes, ultimoDia);
            return data;
        }

        public static byte[] compactar(Stream streamFonte)
        {
            byte[] bytes = null;
            MemoryStream streamDestino = new MemoryStream();
            GZipStream streamCompactado = new GZipStream(streamDestino, CompressionMode.Compress, true);
            streamFonte.Position = 0;
            try
            {
                const int tamanhoBloco = 4096;
                byte[] buffer = new byte[tamanhoBloco + 1];
                int bytesLidos;
                do
                {
                    bytesLidos = streamFonte.Read(buffer, 0, tamanhoBloco);
                    if ((bytesLidos == 0))
                        break;

                    streamCompactado.Write(buffer, 0, bytesLidos);
                }
                while (true);
                bytes = streamDestino.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // ----- Fecha todos os streams 
                streamFonte.Close();
                streamCompactado.Close();
                streamDestino.Close();
            }
            return bytes;
        }

        public static String transformaVetorEmString(Array vetor, char separador)
        {
            StringBuilder r = new StringBuilder();
            foreach (var v in vetor)
            {
                r.AppendFormat("{0}{1}", v, separador);
            }
            return r.Remove(r.Length - 1, 1).ToString();
        }

        public static int calculaDiferencaDatasEmDias(DateTime inicio, DateTime fim)
        {
            return (int)fim.Subtract(inicio).TotalDays;
        }

        public static bool dataValida(DateTime? data)
        {
            return data != null && data.HasValue && data.Value.Year != 0001;
        }
    }
}