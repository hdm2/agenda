using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using Amon.Nucleo.Utils;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Modelo.Ponto;
using Newtonsoft.Json;

namespace WFApp
{
    public partial class DemonstrativoFormConfirmacao : Form
    {
        private readonly HttpClient c;
        private Funcionario funcionario;

        public DemonstrativoFormConfirmacao(String data, Funcionario funcionario)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            HttpClientHandler ch = new HttpClientHandler();
            ch.UseDefaultCredentials = true;
            c = new HttpClient(ch) { BaseAddress = new Uri(ApoioUtils.getStrConfig("Con")) };

            this.funcionario = funcionario;
            this.data.Text = data;
            this.matricula.Text = funcionario.Id;
            this.nome.Text = funcionario.Nome;

            preencherTabela();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void preencherTabela()
        {
            IList<Batida> batidas = JsonConvert.DeserializeObject<IList<Batida>>(
                c.GetAsync(String.Format("servico/ObterBatidasDia/{0}", funcionario.Id))
                    .Result.Content.ReadAsStringAsync()
                    .Result);

            if (batidas.Any()) { 
                var bindingList = new BindingList<Batida>(batidas);
                var source = new BindingSource(bindingList, null);
                dataGridView.DataSource = source;
                horarioBatida.Text = String.Format("Horário: {0:hh\\:mm}h", batidas.Last().Hora);

                #region Configurando as colunas
                dataGridView.Columns["hora"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns["hora"].DisplayIndex = 1;
                dataGridView.Columns["hora"].Name = "Horário de batida";
                dataGridView.Columns["Id"].Visible = false;
                dataGridView.Columns["IdFuncionario"].Visible = false;
                dataGridView.Columns["Data"].Visible = false;
                dataGridView.Columns["Tipo"].Visible = false;
                dataGridView.Columns["Entrada"].Visible = false;
                dataGridView.Columns["Ip"].Visible = false;
                dataGridView.Columns["IdAutorizacao"].Visible = false;
                dataGridView.Columns["LoginAD"].Visible = false;
                #endregion

                dataGridView.RowHeadersVisible = false;
            }
            else
            {
                dataGridView.Visible = false;
                this.labelSemBatidasNoDia.Text = "Você ainda não bateu ponto hoje.";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
