using System;
using System.Collections.Generic;
using System.IO;
using Amon.Nucleo.Utils;
using Amon.PontoE.Servico.Ponto;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {/*
            Dictionary<String, Object> parametros = new Dictionary<string, object>();
            parametros.Add("inicio", new DateTime(2016, 1, 6));
            parametros.Add("fim", new DateTime(2016, 1, 6));
            parametros.Add("hora", new TimeSpan(14, 0, 0));

            //Se as datas forem iguais, considera a zero hora do dia seguinte como data fim
            if (parametros["fim"] == parametros["inicio"])
                parametros["fim"] = ((DateTime) parametros["fim"]).AddDays(1);

            System.Console.WriteLine("Inicio Processamento: {0:dd/MM/yyyy HH:mm}", DateTime.Now);

            String arquivo = ApoioUtils.getStrConfig("arquivo");
            FileInfo arq = new FileInfo(arquivo);
            if (arq.Exists)
            {
                String novoNome = String.Format(@"{0}.{1:yyyyMMddHHmm}", arquivo, DateTime.Now);
                arq.MoveTo(novoNome);
                System.Console.WriteLine("Arquivo: {0}", arquivo);
                System.Console.WriteLine("Movido para: {0}", novoNome);
            }
            MemoryStream linhas = new BatidaServico().obterBatidasArquivo((DateTime)parametros["inicio"], (DateTime)parametros["fim"]);
            File.WriteAllBytes(arquivo, linhas.ToArray());
            System.Console.WriteLine("Criado Novo Arquivo: {0}", arquivo);
            */
            //PopulaPontos popula = new PopulaPontos();
            //popula.popular("39276");
            //popula = new PopulaPontos();
            //popula.popular("37729");
            //popula = new PopulaPontos();
            //popula.popular("15474");
            
            //System.Console.WriteLine("Tamanho da lista: {0}", new AutorizacaoServico().ObterAutorizacoesDoDiaNaoNotificadas("37729"));

            System.Console.WriteLine("Fim Processamento: {0:dd/MM/yyyy HH:mm}", DateTime.Now);
            
            System.Console.ReadKey();
        }
    }
}
