using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Amon.Nucleo.Utils;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Modelo.Ponto;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WFApp
{
    public partial class FrmPrinc : Form
    {
        #region Variáveis locais
        [DllImport("user32")]
        public static extern void LockWorkStation();

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        private String tituloRepVirtual = "REP Virtual";
        readonly NotifyIcon notifyIcon = new NotifyIcon();
        private readonly HttpClient c;
        private Funcionario funcionario;
        private JornadaTrabalho jornada;
        private IList<Batida> batidasDoDia;
        private readonly TimeSpan tempoTolerancia = TimeSpan.FromMinutes(5);
        private const int timeOutBalao = 5000;
        private String ip;
        private String usuario;
        private bool lotacaoNaoBloqueada = true;
        private TimeSpan ultimaRequisicaoBatida = DateTime.Now.TimeOfDay.Subtract(TimeSpan.FromSeconds(11));
        private IList<TimeSpan> intervalos = new List<TimeSpan>();
        private ContextMenu menuContexto;
        private DateTime hora;
        private bool iniciouHora = false;
        private bool jaDeuErro = false;
        private Uri urlServidor;
        private bool autorizacaoNotificada = false;
        private int frequenciaNotificacaoIntervalo;
        //private TimeSpan horarioUltimaNotificacaoIntervalo;
        private bool notificacaoLembreteHoraExtra = false;
        private bool notificacaoDesbloqueioFuturo = false;
        private bool bloqueado = false;
        private String eGerente = null;
        private bool mostrarDemonstrativo;
        private int posicaoBatidaMenuContexto;
        private int periodoPreHoraExtra;
        private bool notificarIntervalo = true;
        private TimeSpan ultimaNotificacao;
        private bool jornadaAlternativaNotificada = false;
        #endregion

        public FrmPrinc()
        {
            HttpClientHandler ch = new HttpClientHandler();
            ch.UseDefaultCredentials = true;
            urlServidor = new Uri(ApoioUtils.getStrConfig("Con"));
            c = new HttpClient(ch) { BaseAddress = urlServidor };
            urlServidor = new Uri(urlServidor.ToString().Remove(urlServidor.ToString().IndexOf("/", 7, StringComparison.Ordinal)));
            
            InitializeComponent();
        }

        private void FrmPrinc_Load(object sender, EventArgs e)
        {
            #region Carrega o ícone da área de notificação
            tituloRepVirtual = String.Format("{0} {1}", tituloRepVirtual, Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));
            notifyIcon.Text = tituloRepVirtual;
            notifyIcon.Icon = new Icon("relogio.ico");
            notifyIcon.Visible = true;
            this.Visible = false;
            #endregion

            #region Cria o menu de contexto
            //Cria o item de menu
            MenuItem demonstrativo = new MenuItem();
            demonstrativo.Text = "Ver demonstrativo de hoje";
            demonstrativo.Click += demonstrativoClique;

            //Cria o item de batida pela web
            MenuItem batidaWeb = new MenuItem();
            batidaWeb.Text = "Bater ponto pelo link web";
            batidaWeb.Click += baterPontoLinkWeb;

            //Cria item para bater o ponto
            MenuItem batida = new MenuItem();
            batida.Text = "Bater ponto agora";
            batida.Click += baterPonto;

            //Cria o menu de contexto
            menuContexto = new ContextMenu();
            menuContexto.MenuItems.Add(demonstrativo);
            menuContexto.MenuItems.Add(batida);
            //menu.MenuItems.Add(batidaWeb);

            //Adiciona o menu de contexto
            notifyIcon.ContextMenu = menuContexto;
            posicaoBatidaMenuContexto = 1;
            #endregion

            #region Obtém usuário atual
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                if (identity == null)
                {
                    MessageBox.Show(this.Owner, "Falha ao obter usuário logado no domínio.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mataReboot();
                    Application.Exit();
                    return;
                }

                //TODO Usuário de teste
                //usuario = Debugger.IsAttached ? "cozela" : identity.Name;
                //usuario = Debugger.IsAttached ? "bneto" : identity.Name;
                usuario = identity.Name;

                if (String.IsNullOrEmpty(usuario))
                {
                    MessageBox.Show(this.Owner, "Falha ao obter usuário logado no domínio.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mataReboot();
                    Application.Exit();
                    return;
                }

                String host = Dns.GetHostName();
                IPAddress[] ips = Dns.GetHostAddresses(host);
                if (ips.Length == 0)
                    ip = "127.0.0.1";
                else
                {
                    foreach (IPAddress ipAddress in ips.Where(ipAddress => ipAddress.AddressFamily != AddressFamily.InterNetworkV6))
                    {
                        ip = ipAddress.ToString();
                        break;
                    }
                    if (String.IsNullOrEmpty(ip))
                        ip = "127.0.0.1";
                }
                usuario = usuario.Substring(usuario.IndexOf(@"\") + 1);
            #endregion

                inicializarProcesso();
                verificacoesSeguranca();
            }
            catch (WebException wex)
            {
                MessageBox.Show(this.Owner, String.Format("Falha ao consultar serviço.\n{0}", wex.Message), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mataReboot();
                Application.Exit();
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(this.Owner, String.Format("Falha ao consultar serviço.\n{0}", ex.InnerException.Message), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mataReboot();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Owner, String.Format("Falha ao consultar serviço.\n{0}", ex.Message), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mataReboot();
                Application.Exit();
            }
        }

        private void inicializarProcesso()
        {
            #region Obtém o funcionário em sessão
            funcionario = JsonConvert.DeserializeObject<Funcionario>(
                    c.GetAsync(String.Format("servico/ObterFuncionarioPorLogin/{0}", usuario))
                        .Result.Content.ReadAsStringAsync()
                        .Result);
            if (funcionario == null || String.IsNullOrEmpty(funcionario.LoginAD))
            {
                MessageBox.Show(this.Owner, String.Format("Falha ao obter funcionário para o login {0}.", usuario), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mataReboot();
                Application.Exit();
                return;
            }
            #endregion

            #region Verifica se o funcionário é gerente (interfere na notificação de lembrete para hora extra)
            if (String.IsNullOrEmpty(eGerente))
            {
                String matricula =
                    c.GetAsync(String.Format("servico/verificaSeEGerente/{0}", funcionario.Id))
                        .Result.Content.ReadAsStringAsync()
                        .Result ?? String.Empty;
                matricula = matricula.Replace("\"", "");
                eGerente = matricula != String.Empty ? "sim" : "nao";
            }
            #endregion

            #region Obtém hora do servidor
            try
            {
                String str = c.GetAsync("servico/obterHora").Result.Content.ReadAsStringAsync().Result;
                str = str.Substring(str.LastIndexOf("T") + 1, str.LastIndexOf(".") - str.LastIndexOf("T") - 1);
                hora = ConverteUtils.sempreConverteDate(str);
                iniciouHora = true;
                timerObservador.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show(this.Owner, "Falha ao obter hora do servidor.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iniciouHora = false;
                mataReboot();
                Application.Exit();
                return;
            }
            #endregion

            #region Obtém jornada do funcionário
            jornada = JsonConvert.DeserializeObject<JornadaTrabalho>(
                c.GetAsync(String.Format("servico/ObterJornadaTrabalho/{0}", funcionario.IdJornada))
                    .Result.Content.ReadAsStringAsync()
                    .Result);
            if (jornada == null)
            {
                MessageBox.Show(this.Owner, String.Format("Falha ao obter jornada para funcionário {0} - {1}.", funcionario.Id, funcionario.Nome), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mataReboot();
                Application.Exit();
                return;
            }
            #endregion

            #region Pega a frequência das notificações de batida na janela de intervalo
            try
            {
                frequenciaNotificacaoIntervalo = JsonConvert.DeserializeObject<Int32>(c.GetAsync("servico/obterFrequenciaNotificacaoIntervalo")
                    .Result.Content.ReadAsStringAsync()
                    .Result);
            }
            catch (Exception)
            {
                //Caso não seja possível obter o parâmetro, o padrão será o tempo da iteração do agente (40 segundos)
                frequenciaNotificacaoIntervalo = timer.Interval; 
            }
            #endregion

            #region Calcula intervalos
            TimeSpan hr = jornada.Inicio;
            for (int i = 0; i < jornada.InterRepeticoes; i++)
            {
                hr = hr.Add(TimeSpan.FromMinutes(50));
                intervalos.Add(hr);
                hr = hr.Add(TimeSpan.FromMinutes(10));
            }
            #endregion

            #region Obtém a frequência de iteração do agente
            try
            {
                int emSegundos = JsonConvert.DeserializeObject<Int32>(c.GetAsync("servico/obterFrequenciaIteracaoAgente")
                    .Result.Content.ReadAsStringAsync()
                    .Result);
                //Converte para milissegundos
                this.timer.Interval = emSegundos*1000;
            }
            catch (Exception)
            {
                //timer.Interval já traz um valor padrão, atribuído em FrmPrinc.Designer.cs
                //Não é necessário tratar a exceção
            }
            #endregion

            #region Obter período pré hora extra
            //Em minutos
            try
            {
                periodoPreHoraExtra =
                    JsonConvert.DeserializeObject<Int32>(c.GetAsync("servico/obterPeriodoPreHoraExtra")
                        .Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                //Define um valor padrão caso haja problema na consulta do parâmetro
                periodoPreHoraExtra = 15;
            }
            #endregion

            #region Verifica se deve ser mostrado o demonstrativo após a batida de ponto
            mostrarDemonstrativo = true;
                /*ConverteUtils.sempreConverteBoleano(JsonConvert.DeserializeObject<string>(
                                                                                        c.GetAsync("servico/deveMostrarDemonstrativoAposBatida")
                                                                                            .Result.Content.ReadAsStringAsync().Result));*/
            #endregion

            #region Obtém batidas do dia
            //batidasDoDia = JsonConvert.DeserializeObject<IList<Batida>>(
            //    c.GetAsync(String.Format("servico/ObterBatidasDia/{0}", funcionario.Id))
            //        .Result.Content.ReadAsStringAsync().Result);
            atualizarListaDeBatidasDoDia();
            #endregion

            defineHorariosDoDia();
        }

        private void verificacoesSeguranca()
        {
            notifyIcon.BalloonTipClicked -= baterPonto;
            TimeSpan terminoIntervalo = TimeSpan.FromHours(23);
            if (batidasDoDia.Count == 2)
                terminoIntervalo = batidasDoDia.Last().Hora.Add(jornada.IntervaloTimeSpan);

            #region Inicializa o funcionário e seus dados, caso necessário
            if (funcionario == null)
                inicializarProcesso();
            #endregion

            #region Pega a hora do servidor
            try
            {
                hora = JsonConvert.DeserializeObject<DateTime>(c.GetAsync("servico/obterHora")
                    .Result.Content.ReadAsStringAsync()
                    .Result);
                iniciouHora = true;
                timerObservador.Enabled = true;
            }
            catch (Exception)
            {
                iniciouHora = false;
            }
            #endregion

            defineTextoPadraoDoDescritivo();

            #region Lembrete hora extra
            if (eGerente == "sim" && !notificacaoLembreteHoraExtra)
            {
                TimeSpan horaLembrete = new TimeSpan(10, 59, 00);
                    /*ConverteUtils.sempreConverteDate(JsonConvert.DeserializeObject<string>(
                                                                c.GetAsync("servico/obterHorarioLembreteHoraExtra")
                                                                    .Result.Content.ReadAsStringAsync().Result)).TimeOfDay;*/

                if (hora.TimeOfDay.Hours == horaLembrete.Hours && hora.TimeOfDay.Minutes == horaLembrete.Minutes)
                {
                    //Define o balão para aparecer durante 2 minutos
                    notifyIcon.ShowBalloonTip(2 * 60 * 1000, tituloRepVirtual,
                        TextoNotificacao.notificacaoHoraExtra, ToolTipIcon.Warning);

                    notifyIcon.BalloonTipClicked += confirmaNotificacaoLembreteHoraExtra;
                    return;
                }
            }
            #endregion

            #region Desconsidera as regras de notificação caso o funcionário não tenha que bater ponto
            if (funcionario.NaoBatePonto)
                return;
            #endregion

            #region Verifica se a lotação é bloqueada
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(funcionario.LotacaoId);
                String lotEncode = Convert.ToBase64String(plainTextBytes);
                lotacaoNaoBloqueada = JsonConvert.DeserializeObject<bool>(c.GetAsync(String.Format("servico/verificaSeBloqueada/{0}", lotEncode))
                                             .Result.Content.ReadAsStringAsync().Result);
                if (lotacaoNaoBloqueada) { 
                    notifyIcon.BalloonTipClosed -= efetuarLogOff;
                    notifyIcon.BalloonTipClicked -= efetuarLogOff;
                    //notifyIcon.BalloonTipClicked -= baterPonto;
                    //notifyIcon.Click -= baterPonto;
                    //notifyIcon.DoubleClick -= baterPonto;
                }
            }
            catch (Exception)
            { }
            #endregion

            #region Consulta e notifica as autorizações do funcionário
            IList<Autorizacao> autorizacoes = new List<Autorizacao>();
            List<Autorizacao> autorizacoesCorrentes = new List<Autorizacao>();

            try
            {
                autorizacoes = JsonConvert.DeserializeObject<IList<Autorizacao>>(
                    c.GetAsync(String.Format("servico/ObterAutorizacoesDoDiaNaoNotificadas/{0}", funcionario.Id))
                        .Result.Content.ReadAsStringAsync()
                        .Result) ?? new List<Autorizacao>();

                if (autorizacoes.Any())
                {
                    //Filtra apenas as autorizações nas quais o horário atual está compreendido
                    autorizacoesCorrentes = autorizacoes.Where(a => hora.TimeOfDay >= a.Inicio && hora.TimeOfDay <= a.Fim).ToList();

                    if (autorizacoesCorrentes.Any())
                    {
                        //Será considerado apenas o desbloqueio mais recente
                        int idUltimaAutorizacao = autorizacoesCorrentes.Max(a => a.Id);
                        Autorizacao ultimaCadastrada = autorizacoesCorrentes.First(a => a.Id == idUltimaAutorizacao);

                        if (!autorizacaoNotificada)
                        {
                            notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                                    String.Format(TextoNotificacao.notificaAutorizacao, ultimaCadastrada.Inicio, ultimaCadastrada.Fim),
                                    ToolTipIcon.Info);

                            notifyIcon.BalloonTipClicked += confirmaNotificacaoDesbloqueio;
                        }

                        habilitarBatida();
                        switch (ultimaCadastrada.IdTipo)
                        {
                            case 2: //2 = hora extra
                                if ((batidasDoDia.Any() && hora.TimeOfDay.Subtract(batidasDoDia.Last().Hora).TotalMinutes > 15) || !batidasDoDia.Any())
                                    habilitarBatida();
                                    //notifyIcon.DoubleClick += baterPonto;
                                break;
                            default: //notifyIcon.DoubleClick += baterPonto;
                                habilitarBatida();
                                break;
                        }

                        //Notifica o tempo restante, se faltarem menos de 5 minutos
                        TimeSpan restante = ultimaCadastrada.Fim.Subtract(hora.TimeOfDay);
                        if (restante <= tempoTolerancia && ultimaCadastrada.Fim > jornada.Fim)
                        { 
                            notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                                String.Format(TextoNotificacao.tempoRestanteAutorizacao, restante.TotalMinutes > 1 ? 
                                                                                         (int) restante.TotalMinutes + " minutos" : 
                                                                                         (int) restante.TotalSeconds + " segundos"),
                                ToolTipIcon.Info);

                            //notifyIcon.BalloonTipClicked += baterPonto;
                        }
                    }
                    else
                    {
                        List<Autorizacao> autorizacoesFuturasNoDia = autorizacoes.Where(a => a.Inicio > hora.TimeOfDay).ToList();
                        if (!notificacaoDesbloqueioFuturo && autorizacoesFuturasNoDia.Any())
                        {
                            int idMaisRecente = autorizacoesFuturasNoDia.Max(a => a.Id);
                            Autorizacao maisRecente = autorizacoesFuturasNoDia.First(a => a.Id == idMaisRecente);
                            notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                                    String.Format(TextoNotificacao.notificacaoNovoDesbloqueio, maisRecente.Inicio, maisRecente.Fim),
                                    ToolTipIcon.Info);

                            //notifyIcon.BalloonTipClicked -= baterPonto;
                            notificacaoDesbloqueioFuturo = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual, "O REP Virtual apresentou um comportamento inesperado. A tentativa de recuperação se dará em alguns segundos.", ToolTipIcon.Error);
                Thread.Sleep(3000);
                Process.Start("WFApp.exe");
            }
            #endregion

            #region Implementa as regras de notificação

            //Remove os eventos de clique no balão
            notifyIcon.BalloonTipClosed -= efetuarLogOff;
            notifyIcon.BalloonTipClicked -= efetuarLogOff;
            notifyIcon.Click -= baterPonto;

            //Quando o número de batidas for 4 e houver uma hora extra corrente, deve-se verificar o período pré-hora extra, definido no parâmetro 'intervaloPreHoraExtra' na base.
            if (batidasDoDia.Count == 4 && autorizacoesCorrentes.Any() && autorizacoesCorrentes.Last().IdTipo == 2 && hora.TimeOfDay.Subtract(batidasDoDia.Last().Hora).TotalMinutes < periodoPreHoraExtra)
            {
                if (funcionario.NaoBatePonto)
                    return;

                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.aguardoHoraExtra, periodoPreHoraExtra), ToolTipIcon.Warning);

                notifyIcon.BalloonTipClicked += efetuarLogOff;
                notifyIcon.BalloonTipClosed += efetuarLogOff;
                Thread.Sleep(timeOutBalao);
                LockWorkStation();
            }

            //Quando o número de batidas for igual a 4 e ainda estiver na tolerância a máquina não deve ser bloqueada
            if (batidasDoDia.Count == 4 && jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))) >= hora.TimeOfDay)
            {
                if (funcionario.NaoBatePonto)
                    return;

                if (!autorizacoesCorrentes.Any())
                {
                    int minutosDiferenca = (int)jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))).Subtract(hora.TimeOfDay).TotalMinutes;
                    String unidade = minutosDiferenca == 1 ? " minuto" : " minutos";
                    notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                        String.Format(TextoNotificacao.tempoRestanteFim, minutosDiferenca >= 1 ?
                                                                         minutosDiferenca + unidade :
                                                                         (int)jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))).Subtract(hora.TimeOfDay).TotalSeconds + " segundos"
                        ), ToolTipIcon.Warning);

                    //notifyIcon.BalloonTipClicked -= baterPonto;
                    notifyIcon.BalloonTipClosed -= efetuarLogOff;
                    notifyIcon.BalloonTipClicked -= efetuarLogOff;
                }
                if (batidasDoDia.Count == 4)
                    desabilitarBatida();
                else
                    habilitarBatida();

                bloqueado = false;
                return;
            }
            
            //Não pode usar a máquina antes do início mínimo da jornada
            if (jornada.Inicio.Subtract(tempoTolerancia) > hora.TimeOfDay && !autorizacoesCorrentes.Any())
            {
                if (funcionario.NaoBatePonto || lotacaoNaoBloqueada)
                    return;

                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.preJornada, jornada.Inicio), ToolTipIcon.Warning);
                notifyIcon.BalloonTipClicked += efetuarLogOff;
                notifyIcon.BalloonTipClosed += efetuarLogOff;
                desabilitarBatida();
                bloqueado = true;
                
                return;
            }

            //Quando o número de batidas for igual a 4, tiver passado o tempo máximo de saída e o funcionário não estiver autorizado, o sistema bloqueia.
            if (batidasDoDia.Count == 4 && jornada.Fim.Add(tempoTolerancia) < hora.TimeOfDay && !autorizacoesCorrentes.Any())
            {
                if (funcionario.NaoBatePonto || lotacaoNaoBloqueada)
                    return;

                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.fimJornada, batidasDoDia[3].Hora.ToString().Substring(0, 5)), ToolTipIcon.Warning);

                //notifyIcon.DoubleClick -= baterPonto;
                //notifyIcon.BalloonTipClicked += efetuarLogOff;
                //notifyIcon.BalloonTipClicked -= baterPonto;
                desabilitarBatida();
                notifyIcon.BalloonTipClosed += efetuarLogOff;
                bloqueado = true;
                return;
            }

            //Alerta para proximidade do fim da jornada
            if (jornada.Fim.Subtract(tempoTolerancia) <= hora.TimeOfDay && jornada.Fim >= hora.TimeOfDay)
            {
                if (funcionario.NaoBatePonto)
                    return;
                int minutosDiferenca = (int) jornada.Fim.Subtract(hora.TimeOfDay).TotalMinutes;
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.proximidadeFim, jornada.Fim, minutosDiferenca > 1 ? 
                                                                                minutosDiferenca + " minutos" : 
                                                                                (int) jornada.Fim.Subtract(hora.TimeOfDay).TotalSeconds + " segundos"
                    ), ToolTipIcon.Warning);

                //notifyIcon.BalloonTipClosed -= baterPonto;
                notifyIcon.BalloonTipClicked += baterPonto;
                if (batidasDoDia.Count == 4)
                    desabilitarBatida();
                else
                    habilitarBatida();

                bloqueado = false;
                return;
            }

            //Alerta para proximidade do fim da jornada (já dentro da tolerância)
            if (jornada.Fim.Subtract(tempoTolerancia) <= hora.TimeOfDay && jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))) >= hora.TimeOfDay && batidasDoDia.Count == 3)
            {
                if (funcionario.NaoBatePonto)
                    return;

                int minutosDiferenca = (int) jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))).Subtract(hora.TimeOfDay).TotalMinutes;
                String unidade = minutosDiferenca == 1 ? " minuto" : " minutos";
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.naToleranciaFim, jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))), minutosDiferenca >= 1 ? 
                                                                                                      minutosDiferenca + unidade :
                                                                                                      (int)jornada.Fim.Add(tempoTolerancia.Add(TimeSpan.FromSeconds(59))).Subtract(hora.TimeOfDay).TotalSeconds + " segundos"
                    ), ToolTipIcon.Warning);
                
                notifyIcon.BalloonTipClicked += baterPonto;
                if (batidasDoDia.Count == 4)
                    desabilitarBatida();
                else
                    habilitarBatida();

                //notifyIcon.DoubleClick += baterPonto;
                bloqueado = false;
                return;
            }

            //Não pode usar a máquina depois do fim da jornada
            if (jornada.Fim.Add(tempoTolerancia) < hora.TimeOfDay && !autorizacoesCorrentes.Any())
            {
                if (funcionario.NaoBatePonto || lotacaoNaoBloqueada)
                    return;
              
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.aposFim, jornada.Fim), ToolTipIcon.Warning);
                desabilitarBatida();
                notifyIcon.BalloonTipClosed += efetuarLogOff;
                notifyIcon.BalloonTipClicked += efetuarLogOff;
                bloqueado = true;
               
                return;
            }

            //Sugestão bater o ponto no início da jornada
            if (!batidasDoDia.Any() && hora.TimeOfDay >= jornada.Inicio.Subtract(TimeSpan.FromMinutes(5)) && hora.TimeOfDay < jornada.MaxIntervalo)
            {
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.inicio, jornada.Inicio), ToolTipIcon.Warning);

                notifyIcon.BalloonTipClicked += baterPonto;
                habilitarBatida();
                bloqueado = false;
                return;
            }

            //Alerta já pode tirar intervalo
            if (batidasDoDia.Count == 1 && hora.TimeOfDay >= jornada.MinIntervalo && hora.TimeOfDay <= jornada.MaxIntervalo && hora.TimeOfDay > batidasDoDia.Last().Hora.Add(TimeSpan.FromMinutes(10)))
            {
                //if (horarioUltimaNotificacaoIntervalo == null || (int) horarioUltimaNotificacaoIntervalo.TotalSeconds == 0)
                //    horarioUltimaNotificacaoIntervalo = hora.TimeOfDay;

                //int tempoAposUltimaNotificacaoIntervalo = (int) hora.TimeOfDay.Subtract(horarioUltimaNotificacaoIntervalo).TotalSeconds;
                //bool notificar = frequenciaNotificacaoIntervalo <= tempoAposUltimaNotificacaoIntervalo;

                //if (notificar || tempoAposUltimaNotificacaoIntervalo == 0)
                if (notificarIntervalo)
                {
                    notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                        String.Format(TextoNotificacao.aposIntervaloMinimo, jornada.MaxIntervalo), ToolTipIcon.Warning);
                    
                    notifyIcon.BalloonTipClicked += baterPonto;
                    //horarioUltimaNotificacaoIntervalo = hora.TimeOfDay;
                    notificarIntervalo = false;
                }

                notifyIcon.Text = "Você já pode bater o ponto do intervalo.";
                
                //notifyIcon.BalloonTipClicked += baterPonto;
                //notifyIcon.DoubleClick += baterPonto;
                habilitarBatida();
                notifyIcon.Click += baterPonto;
                bloqueado = false;
                return;
            }

            foreach (TimeSpan i in intervalos)
            {
                if (hora.TimeOfDay >= i && hora.TimeOfDay <= i.Add(TimeSpan.FromMinutes(1)))
                {
                    notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                        String.Format(TextoNotificacao.inicioIntervaloProgramado, jornada.MaxIntervalo), ToolTipIcon.Warning);
                    
                    //notifyIcon.BalloonTipClicked += baterPonto;
                    return;
                }

                if ((batidasDoDia.Count % 2 == 0) && hora.TimeOfDay >= i.Add(TimeSpan.FromMinutes(11)) && hora.TimeOfDay <= i.Add(TimeSpan.FromMinutes(12)))
                {
                    notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                        String.Format(TextoNotificacao.retornoIntervaloProgramado, jornada.MaxIntervalo), ToolTipIcon.Warning);
                    
                    //notifyIcon.BalloonTipClicked += baterPonto;
                    return;
                }
            }

            //Alerta para proximidade do horário de intervalo
            if (batidasDoDia.Count == 1 && jornada.MinIntervalo.Subtract(tempoTolerancia) < hora.TimeOfDay && jornada.MinIntervalo > hora.TimeOfDay)
            {
                if (ultimaNotificacao == null) 
                    ultimaNotificacao = hora.TimeOfDay;

                if (hora.TimeOfDay.Subtract(ultimaNotificacao).TotalMinutes > 1) { 
                    int minutosDiferenca = (int) jornada.MinIntervalo.Subtract(hora.TimeOfDay).Add(TimeSpan.FromSeconds(hora.TimeOfDay.Seconds)).TotalMinutes;
                    String unidade = minutosDiferenca == 1 ? " minuto" : " minutos";
                    notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                        String.Format(TextoNotificacao.proximidadeIntervalo, minutosDiferenca >= 1 ?
                                                                             minutosDiferenca + unidade :
                                                                             (int) jornada.MinIntervalo.Subtract(hora.TimeOfDay).TotalSeconds + " segundos"
                        ), ToolTipIcon.Warning);

                    ultimaNotificacao = hora.TimeOfDay;
                }
                //notifyIcon.DoubleClick -= baterPonto;
                desabilitarBatida();
                bloqueado = false;
                return;
            }

            //Atingiu o horário máximo de intervalo e será desconectado. Permite bater o ponto neste momento.
            if (batidasDoDia.Count == 1 && jornada.MaxIntervalo < hora.TimeOfDay)
            {
                if (lotacaoNaoBloqueada)
                    return;
                
                //String msg = TextoNotificacao.aposIntervaloMaximo;
                //if (!lotacaoNaoBloqueada)
                //    msg = String.Format(TextoNotificacao.lembreteIntervalo, jornada.Intervalo);

                int tempoIteracaoAgente = this.timer.Interval;
                notifyIcon.ShowBalloonTip(tempoIteracaoAgente, tituloRepVirtual, String.Format(TextoNotificacao.aposIntervaloMaximo, jornada.Intervalo), ToolTipIcon.Warning);

                //Pega a hora atual e soma mais uma iteração do agente para que o funcionário bata o ponto
                if (hora.TimeOfDay > jornada.MaxIntervalo.Add(TimeSpan.FromMilliseconds(tempoIteracaoAgente)))
                    notifyIcon.BalloonTipClosed += efetuarLogOff;

                //notifyIcon.DoubleClick += baterPonto;
                notifyIcon.BalloonTipClicked += baterPonto;
                notifyIcon.Click += baterPonto;
                habilitarBatida();
                bloqueado = false;
                return;
            }

            //Esta dentro do range de intervalo e não permite o uso da máquina
            if (batidasDoDia.Count == 2 && terminoIntervalo > hora.TimeOfDay)
            {
                if (lotacaoNaoBloqueada)
                    return;

                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual,
                    String.Format(TextoNotificacao.duranteIntervalo, batidasDoDia.Last().Hora.Add(jornada.IntervaloTimeSpan)),
                    ToolTipIcon.Warning);

                //Thread.Sleep(timeOutBalao);
                //notifyIcon.BalloonTipClicked += efetuarLogOff;
                desabilitarBatida();
                notifyIcon.Click -= baterPonto;
                notifyIcon.BalloonTipClosed += efetuarLogOff;
                notifyIcon.BalloonTipClicked += efetuarLogOff;
                bloqueado = true;
                return;
            }

            //Já pode usar a máquina depois do intervalo, mas não bateu o ponto. Permite a batida do ponto
            if (batidasDoDia.Count == 2 && terminoIntervalo <= hora.TimeOfDay)
            {
                String msg = (hora.TimeOfDay >= terminoIntervalo) ? 
                    String.Format(TextoNotificacao.aposIntervalo, terminoIntervalo) :
                    String.Format(TextoNotificacao.retornoIntervaloNaTolerancia, terminoIntervalo.Subtract(hora.TimeOfDay));

                //notifyIcon.BalloonTipClicked -= baterPonto;
                //notifyIcon.BalloonTipClosed -= efetuarLogOff;
                defineTextoPadraoDoDescritivo();
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual, msg, ToolTipIcon.Warning);
                notifyIcon.BalloonTipClicked -= efetuarLogOff;
                notifyIcon.BalloonTipClicked += baterPonto;
                notifyIcon.Click += baterPonto;
                habilitarBatida();
                bloqueado = false;
                return;
            }
            #endregion
        }

        private void defineTextoPadraoDoDescritivo(){
            try
            {
                notifyIcon.Text = String.Format("{0}\n{1} {2}\nHora certa: {3:HH:mm}h", tituloRepVirtual, funcionario.Id, funcionario.Nome, hora);
            }
            catch (Exception)
            {
                //Se o texto do notifyIcon ultrapassar o limite de 64 caracteres, será mostrado apenas o primeiro nome do funcionário
                String descritivo = String.Format("{0}\n{1} {2}\n{3:HH:mm}h", tituloRepVirtual, funcionario.Id, funcionario.Nome, hora);

                if (descritivo.Length >= 64)
                    descritivo = String.Format("{0}\n{1} {2}", tituloRepVirtual, funcionario.Id, funcionario.Nome.Split(' '));

                notifyIcon.Text = descritivo;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                atualizarListaDeBatidasDoDia();
                verificacoesSeguranca();
                jaDeuErro = false;
            }
            catch (Exception ex)
            {
                if (jaDeuErro)
                    return;

                jaDeuErro = true;
                MessageBox.Show(this.Owner, String.Format("Falha na consulta ao serviço.\n{0}", ex.Message), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void baterPonto(object sender, EventArgs e)
        {
            if (!baterPonto())
                return;
            
            if (bloqueado) { 
                Thread.Sleep(5000);
                LockWorkStation();
            }
        }

        private void demonstrativoClique(object Sender, EventArgs e)
        {
            new DemonstrativoForm(hora, funcionario).Show();
        }

        private void baterPontoLinkWeb(object Sender, EventArgs e)
        {
            String url = Debugger.IsAttached ? "localhost:63583/Autenticacao/baterponto" : urlServidor.ToString() + "SRP";
            String navegador = identificarNavegadorWEB();
            //navegador = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            if (string.IsNullOrEmpty(navegador))
                MessageBox.Show("Não foi possível abrir o link automaticamente! Acesse seu navegador e o endereço do REP Virtual para bater o ponto.");
            else
                Process.Start(navegador, url);
        }

        private String identificarNavegadorWEB()
        {
            string browserPath = string.Empty;
            
            try
            {
                RegistryKey browserKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                if (browserKey == null)
                    browserKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http", false); ;

                if (browserKey != null)
                {
                    browserPath = (browserKey.GetValue(null) as string).ToLower().Replace("\"", "");

                    if (!browserPath.EndsWith("exe"))
                        browserPath = browserPath.Substring(0, browserPath.LastIndexOf(".exe") + 4);

                    browserKey.Close();
                }
            }
            catch
            {
                return string.Empty;
            }
            //return browserPath;
            return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        }

        private void notificaJornadaAlternativa()
        {
            habilitarBatida();
            if (!jornadaAlternativaNotificada)
            {
                notifyIcon.ShowBalloonTip(timeOutBalao, tituloRepVirtual, TextoNotificacao.temJornadaAlternativa, ToolTipIcon.Warning);
                jornadaAlternativaNotificada = true;
                return;
            }
        }

        private void efetuarLogOff(object sender, EventArgs e)
        {
            //if (!Debugger.IsAttached)
                LockWorkStation();
            //else
            //    MessageBox.Show("BLOQUEIO!");
        }

        private void confirmaNotificacaoLembreteHoraExtra(object sender, EventArgs e)
        {
            notificacaoLembreteHoraExtra = true;
        }
        
        private void confirmaNotificacaoDesbloqueio(object sender, EventArgs e)
        {
            autorizacaoNotificada = true;
        }
        
        protected void logOffWindows()
        {
            if (!lotacaoNaoBloqueada)
                ExitWindowsEx(0, 0);            
        }

        protected void desabilitarBatida()
        {
            menuContexto.MenuItems[posicaoBatidaMenuContexto].Enabled = false;
        }

        protected void habilitarBatida()
        {
            menuContexto.MenuItems[posicaoBatidaMenuContexto].Enabled = true;
        }

        private void defineHorariosDoDia()
        {
            //if (batidasDoDia.Count == 1)
            //{
                //Define a jornada que será assumida no comportamento do ponto a partir da primeira batida
                jornada = JsonConvert.DeserializeObject<JornadaTrabalho>(
                    c.GetAsync(String.Format("servico/IdentificarJornadaTrabalho/{0}?horario={1:hh\\:mm\\:ss}", funcionario.Id, batidasDoDia.Any() ? batidasDoDia.First().Hora : hora.TimeOfDay))
                        .Result.Content.ReadAsStringAsync()
                        .Result);
                funcionario.IdJornada = jornada.Id;
            //}
        }

        protected void atualizarListaDeBatidasDoDia()
        {
            batidasDoDia = JsonConvert.DeserializeObject<IList<Batida>>(
                c.GetAsync(String.Format("servico/ObterBatidasDia/{0}", funcionario.Id))
                    .Result.Content.ReadAsStringAsync().Result);
        }

        //O retorno do tipo bool indica se a máquina deve ou não ser bloqueada após a batida (false = não bloquear)
        protected bool baterPonto()
        {
            TimeSpan momentoClique = ultimaRequisicaoBatida = hora.TimeOfDay;

            if (bloqueado)
                return true;

            //Estrutura para manter o MessageBox em primeiro plan
            DialogResult result = MessageBox.Show(
                new Form { 
                   TopMost = true
                }, String.Format("Você deseja bater o seu ponto?\nHorário de batida: {0}h", momentoClique.ToString().Substring(0, 5)), "Bater Ponto", MessageBoxButtons.YesNo, MessageBoxIcon.Question
            );
            
            if (result == DialogResult.Yes)
            {
                #region Trata a confirmação e registra a batida de ponto
                try
                {
                    String r = c.GetAsync(String.Format("servico/RegistrarPonto/{0}?ip={1}&momentoClique={2:hh\\:mm\\:ss}&loginAdSessao={3}", funcionario.Id, ip, momentoClique, funcionario.LoginAD)).Result.Content.ReadAsStringAsync().Result;

                    atualizarListaDeBatidasDoDia();

                    defineHorariosDoDia();

                    #region Notifica a batida de ponto
                    if (r.StartsWith("\"OK-") || (r == "\"OK\""))
                    {
                        r = r.Replace("\"OK-", "").Replace("\"", "");
                        this.BringToFront();
                        
                        #region Define como será o feedback da batida (demonstrativo ou caixa de diálogo simples, conforme parâmetro 'deveMostrarDemonstrativoAposBatida' na base)
                        if (mostrarDemonstrativo)
                            new DemonstrativoFormConfirmacao(String.Format("{0:dd/MM/yyyy}", hora.Date), funcionario).Show();
                        else
                            MessageBox.Show(this.Owner, String.Format("Ponto registrado com sucesso às {0}h.", r), tituloRepVirtual, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        #endregion
                        
                        if (batidasDoDia.Count % 2 == 0)
                            //Retorna se corresponde à última batida porque a máquina, na saída, só deve ser bloqueada após a tolerância pós jornada
                            return batidasDoDia.Count != 4;
                        else
                            return false;
                    }
                    else { 
                        MessageBox.Show(this.Owner, r.Replace("\"", ""), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.Owner, ex.Message.Replace("\"", ""), "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion
            }
            else
            {
                MessageBox.Show("Você não bateu o ponto.", tituloRepVirtual, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private void timerObservador_Tick(object sender, EventArgs e)
        {
            Program.ObservaProcessos();

            if (iniciouHora)
                hora = hora.AddSeconds(1);
        }

        private void mataReboot()
        {
            timerObservador.Enabled = false;

            Process.GetProcesses().Where(p => p.ProcessName == "WFReboot").ToList().ForEach(p => p.Kill());

            //Process[] processes = Process.GetProcesses();
            //foreach (Process process in processes)
            //{
            //    if (process.ProcessName == "WFReboot") 
            //        process.Kill();                
            //}            
        }
    }
}