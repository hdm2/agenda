using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Amon.Nucleo.Utils;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Modelo.Ponto;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WFApp
{
    public partial class DemonstrativoForm : Form
    {
        private readonly HttpClient c;
        private Funcionario funcionario;

        public DemonstrativoForm(DateTime dataHora, Funcionario funcionario)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            HttpClientHandler ch = new HttpClientHandler();
            ch.UseDefaultCredentials = true;
            c = new HttpClient(ch) { BaseAddress = new Uri(ApoioUtils.getStrConfig("Con")) };

            this.funcionario = funcionario;
            this.data.Text = String.Format("{0:dd/MM/yyyy}", dataHora.Date);
            this.matricula.Text = funcionario.Id;
            this.nome.Text = funcionario.Nome;
            this.horaCerta.Text = String.Format("Hora certa: {0:hh\\:mm}h", dataHora.TimeOfDay);
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

            if (batidas.Count > 0) { 
                var bindingList = new BindingList<Batida>(batidas);
                var source = new BindingSource(bindingList, null);
                dataGridView.DataSource = source;

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

        private void horaCerta_Click(object sender, EventArgs e)
        {

        }
    }
}
