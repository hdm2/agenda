using System.IO;
using System.Reflection;
using Amon.PontoE.Modelo.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using Amon.PontoE.Persistencia.Ponto;
using Amon.Nucleo.Persistencia;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Persistencia;
using Amon.PontoE.Servico.Cadastro;

namespace Amon.PontoE.Servico.Ponto
{
    public class BatidaServico
    {
        private readonly BatidaDAO dao;

        #region Construtores

        public BatidaServico()
        {
            dao = new BatidaDAO();
        }

        public BatidaServico(ISimplesDao masterDAO)
        {
            dao = masterDAO.obterOutroDao<BatidaDAO>();
        }

        #endregion

        public IList<Batida> obterBatidasDoDia(String idFuncionario)
        {
            return dao.obterBatidasDoDia(idFuncionario);
        }

        public MemoryStream obterBatidasArquivo(DateTime inicio, DateTime fim)
        {
            return dao.obterBatidasArquivo(inicio, fim);
        }

        public IList<Batida> obterBatidasPorData(String idFuncionario, DateTime inicio, DateTime fim)
        {
            return dao.obterBatidasPorData(idFuncionario, inicio, fim);
        }

        //Batida pelo WEB
        public void registrarPonto(String idFuncionario, String ip, String loginAdSessao, bool forcarBatida = false)
        {
            if (forcarBatida)
            {
                dao.registrarBatida(idFuncionario, ip, loginAdSessao);
                return;
            }

            //Verifica se a lotação é desbloqueada
            Funcionario f = new FuncionarioServico().obter(idFuncionario);
            List<String> idsLotacoes = new LotacaoServico().obterLotacoesNaoBloqueadas().Select(l => l.Id).ToList();
            if (idsLotacoes.Contains(f.LotacaoId)) { 
                dao.registrarBatida(idFuncionario, ip, loginAdSessao);
                return;
            }

            //Verifica o intervalo entre batidas
            int periodoEntreBatidas = Int32.Parse(new ParametroConfiguracaoDAO().obterParametroConfiguracao("REP Virtual", "agente", "intervaloEntreBatidas").First().parametro);
            Batida batidaRecente = dao.detectarBatidaRecente(idFuncionario, periodoEntreBatidas);
            if (batidaRecente != null)
                throw new AmbiguousMatchException(String.Format("Você bateu ponto há menos de {0} minutos, às {1:hh\\:mm}h.", periodoEntreBatidas, batidaRecente.Hora));

            //Obtém as batida do dia
            IList<Batida> batidasDoDia = new BatidaServico().obterBatidasDoDia(idFuncionario);
            //Obtém o horário da primeira batida, caso já tenha ocorrido. Caso contrário, assume-se o horário atual como referência.
            TimeSpan horaPrimeiraBatida = batidasDoDia.Any() ? batidasDoDia.First().Hora : DateTime.Now.TimeOfDay;
            //Define a jornada que será assumida no dia
            JornadaTrabalho jornada = new JornadaTrabalhoServico().IdentificarJornadaTrabalho(idFuncionario, String.Format("{0:hh\\:mm\\:ss}", horaPrimeiraBatida));
           
            //Verifica se o horário atual está dentro da jornada definida
            int inicio = DateTime.Now.TimeOfDay.CompareTo(jornada.Inicio.Subtract(TimeSpan.FromMinutes(5)));
            int fim = DateTime.Now.TimeOfDay.CompareTo(jornada.Fim.Add(TimeSpan.FromMinutes(5)));
            if (inicio >= 0 && fim <= 0)
            {
                dao.registrarBatida(idFuncionario, ip, loginAdSessao);
                return;
            }

            //Verifica o intervalo antes da hora extra
            List<Autorizacao> autorizacoes = new AutorizacaoServico().obterTodasDoDiaPorFuncionario(idFuncionario).ToList();
            if (autorizacoes.Any())
            {
                //Autorizacao maisRecente = autorizacoes.Last();
                //autorizacoes.Any(a => a.Inicio <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay <= a.Fim)

                if (batidasDoDia.Any())
                {
                    int minutosAguardo = Int32.Parse(
                        new ParametroConfiguracaoDAO().obterParametroConfiguracao("REP Virtual", "web", "intervaloPreHoraExtra").First().parametro);

                    if (jornada.Fim < DateTime.Now.TimeOfDay &&
                        (int) DateTime.Now.TimeOfDay.Subtract(batidasDoDia.Last().Hora).TotalMinutes < minutosAguardo)
                    {
                        throw new AmbiguousMatchException(
                            String.Format("Você só poderá iniciar sua hora extra {0} minutos após sua saída.",
                                minutosAguardo));
                    }
                }

                //Trata quando o horário atual está dentro do período de desbloqueio. Cede mais 59 segundos para a batida do ponto.
                if (autorizacoes.Any(a => a.Inicio <= DateTime.Now.TimeOfDay && a.Fim.Add(TimeSpan.FromSeconds(59)) >= DateTime.Now.TimeOfDay))
                {
                    dao.registrarBatida(idFuncionario, ip, loginAdSessao);
                    return;
                }
            }

            //Verifica se cumpriu o intervalo completo
            //if (batidasDoDia.Any()) { 
            //    TimeSpan horaUltimaBatida = batidasDoDia.Last().Hora;
            //    int diferenca = (int) DateTime.Now.TimeOfDay.Subtract(horaUltimaBatida).TotalMinutes;
            //    if (batidasDoDia.Count == 2 && 
            //        estaNaJanelaIntervalo(jornada, horaUltimaBatida) &&
            //        jornada.Intervalo > diferenca)
            //    {
            //        throw new AmbiguousMatchException(
            //                    String.Format("Você deve tirar seus {0} minutos de intervalo antes de retornar.",
            //                        jornada.Intervalo));
            //    }
            //}

            if (DateTime.Now.TimeOfDay > jornada.Fim.Add(TimeSpan.FromMinutes(6)) || 
                DateTime.Now.TimeOfDay < jornada.Inicio.Subtract(TimeSpan.FromMinutes(5)))
                throw new AmbiguousMatchException("Você não pode bater ponto neste horário!");

            dao.registrarBatida(idFuncionario, ip, loginAdSessao);
        }

        //Batida pelo AGENTE
        public void registrarPonto(String idFuncionario, String ip, String momentoClique, String loginAdSessao, bool forcarBatida = false)
        {
            if (!forcarBatida)
            {
                int periodoEntreBatidas = Int32.Parse(new ParametroConfiguracaoDAO().obterParametroConfiguracao("REP Virtual", "agente", "intervaloEntreBatidas").First().parametro);
                Batida b = dao.detectarBatidaRecente(idFuncionario, periodoEntreBatidas);
                if (b != null)
                    throw new AmbiguousMatchException(String.Format("Você bateu ponto há menos de {0} minutos, às {1:hh\\:mm}.", periodoEntreBatidas, b.Hora));
            }

            dao.registrarBatida(idFuncionario, ip, momentoClique, loginAdSessao);
        }
    }
}