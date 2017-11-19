using System.Data;
using System.IO;
using Amon.Persistencia;
using Amon.Persistencia.Construtor;
using Amon.PontoE.Modelo.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amon.PontoE.Persistencia.Ponto
{
    public class BatidaDAO : SimplesAbstractDAO<Batida>
    {
        protected readonly SqlConstrutor<Batida> sqlConstrutor;

        #region Construtores
        public BatidaDAO() : base()
        {
            sqlConstrutor = SqlConstrutor<Batida>.iniciar(obterTabela());
        }

        public BatidaDAO(AcessaDados ad) : base(ad)
        {
            sqlConstrutor = SqlConstrutor<Batida>.iniciar(obterTabela());
        }
        #endregion
        
        protected override string obterTabela()
        {
            return "Batida";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public IList<Batida> obterBatidasDoDia(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Batida> batidas = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select *
                                from Batida
                                where data = @data and idFuncionario = @idFuncionario
                                order by data desc, hora")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("data", DateTime.Today)
                .obterLista<Batida>();

            if (controleInterno)
                ad.fechaConexao();

            return batidas;
        }

        public IList<Batida> obterBatidasPorData(String idFuncionario, DateTime inicio, DateTime fim)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            List<Batida> batidas = (List<Batida>) AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select *
                                from Batida
                                where data between @inicio and @fim 
                                and idFuncionario = @idFuncionario
                                order by data desc, hora")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("inicio", inicio)
                .adicionaParametro("fim", fim)
                .obterLista<Batida>();
        
            if (controleInterno)
                ad.fechaConexao();

            return batidas;
        }

        public Batida detectarBatidaRecente(String idFuncionario, int periodoEntreBatidas)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            TimeSpan horaAtualPrimeiroSegundo = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            TimeSpan horaAtualUltimoSegundo = new TimeSpan(horaAtualPrimeiroSegundo.Hours, horaAtualPrimeiroSegundo.Minutes, 59);

            IList<Batida> r = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(@"select * from batida where idfuncionario = '{0}' and data = CAST('{1:yyyyMMdd}' as date) and 
                                        hora BETWEEN cast(dateadd(MINUTE, cast('-{2}' as int), cast('{3}' as time(0))) as time(0)) AND cast('{4}' as time(0))",
                             idFuncionario, DateTime.Today, periodoEntreBatidas, horaAtualPrimeiroSegundo, horaAtualUltimoSegundo)
                .obterLista<Batida>();

            if (controleInterno)
                ad.fechaConexao();

            return r.FirstOrDefault();
        }

        //Batida pelo Web
        public void registrarBatida(String idFuncionario, String ip, String loginAdSessao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            Batida b = new Batida
            {
                IdFuncionario = idFuncionario,
                Data = DateTime.Today,
                Hora = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                Ip = ip,
                LoginAD = loginAdSessao
            };

            incluir(b);

            if (controleInterno)
                ad.fechaConexao();
        }

        //Batida pelo AGENTE
        public void registrarBatida(String idFuncionario, String ip, String momentoClique, String loginAdSessao)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            String[] valores = momentoClique.Split(':');

            Batida b = new Batida
            {
                IdFuncionario = idFuncionario,
                Data = DateTime.Today,
                Hora = new TimeSpan(Convert.ToInt32(valores[0]), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[2])),
                Ip = ip,
                LoginAD = loginAdSessao
            };

            incluir(b);

            if (controleInterno)
                ad.fechaConexao();
        }

        public MemoryStream obterBatidasArquivo(DateTime inicio, DateTime fim)
        {
            const String sql = @"select '+086' + Hora + Dia + Mes + Ano + Chapa + cnpj as linha
                            from (
	                            select replace(cast(hora as varchar(5)), ':','') Hora,
		                            CASE 
			                            WHEN DAY(data) < 10 THEN '0' + cast(DAY(data) as varchar(2))
			                            ELSE cast(DAY(data) as varchar(2))
		                            END Dia,
		                            CASE 
			                            WHEN MONTH(data) < 10 THEN '0' + cast(MONTH(data) as varchar(2))
			                            ELSE cast(MONTH(data) as varchar(2))
		                            END Mes,
		                            Substring(cast(YEAR(data) as varchar), 3, 4) Ano,
		                            b.idFuncionario Chapa,
									f.cnpj
	                            from Batida b
								INNER JOIN FuncionarioBanpara f on (b.idFuncionario = f.id)
	                            where data between @data and @dataFim
                            )tmp
                            --where (('+086' + Hora + Dia + Mes + Ano + Chapa + cnpj) is not null)
                            order by Chapa, Hora, Dia, Mes, Ano";

            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IDataReader dr = AcessaDadosConstrutor.iniciar(ad)
                                                  .adicionaSQL(sql)
                                                  .adicionaParametro("data", inicio)
                                                  .adicionaParametro("dataFim", fim)
                                                  .obterDataReader();

            MemoryStream ms = new MemoryStream();
            TextWriter lista = new StreamWriter(ms);
            while (dr.Read())
            {
                lista.WriteLine(dr.GetString(0));
            }
            dr.Close();
            lista.Flush();

            if (controleInterno)
                ad.fechaConexao();
            return ms;
        }

        //VERIFICA SE ESTÁ NO LIMITE DE TOLERÂNCIA DE 5'
        public Batida obterBatidaTolereancia(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Batida> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(" select FB.id, FB.NOME, JT.inicio, JT.fim  from FuncionarioBanpara FB ")
                .adicionaSQL(" inner join JornadaTrabalhoBanpara JT on ")
                .adicionaSQL(" (FB.idJornada = JT.id) ")
                .adicionaSQL(" where FB.id = @idFuncionario ")
                .adicionaSQL(" and (JT.inicio >= CAST(DATEADD(MINUTE, -5, cast(@horaAgora as datetime))as time(0)) ")
                .adicionaSQL(" and JT.inicio <= CAST(DATEADD(MINUTE, 5+1, cast(@horaAgora as datetime))as time(0)) )")
                .adicionaSQL(" OR (JT.fim >= CAST(DATEADD(MINUTE, -5, cast(@horaAgora as datetime))as time(0)) ")
                .adicionaSQL(" and JT.fim <= CAST(DATEADD(MINUTE, 5+1, cast(@horaAgora as datetime))as time(0)) )")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("horaAgora", DateTime.Now.TimeOfDay)
                .obterLista<Batida>();

            if (controleInterno)
                ad.fechaConexao();

            return lista.FirstOrDefault();
        }

        //VERIFICAR SE O USUÁRIO POSSUI DESBLOQUEIO
        public Batida obterAutorizacoesDesbloqueioDoDia(String idFuncionario)
        {
            bool controleInterno = ad.conexaoFechada();
            if (controleInterno)
                ad.abreConexao();

            IList<Batida> lista = AcessaDadosConstrutor.iniciar(ad)
                .adicionaSQL(" select idFuncionario, data, inicio, fim ")
                .adicionaSQL(" from Autorizacao ")
                .adicionaSQL(" where idFuncionario = @idFuncionario ")
                .adicionaSQL(" and data = @data ")
                .adicionaParametro("idFuncionario", idFuncionario)
                .adicionaParametro("data", DateTime.Today)
                .obterLista<Batida>();

            if (controleInterno)
                ad.fechaConexao();

            return lista.FirstOrDefault();
        }
    }
}