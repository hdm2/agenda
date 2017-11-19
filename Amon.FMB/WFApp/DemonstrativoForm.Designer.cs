namespace WFApp
{
    partial class DemonstrativoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemonstrativoForm));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.data = new System.Windows.Forms.Label();
            this.matricula = new System.Windows.Forms.Label();
            this.nome = new System.Windows.Forms.Label();
            this.titulo = new System.Windows.Forms.Label();
            this.labelSemBatidasNoDia = new System.Windows.Forms.Label();
            this.horaCerta = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 73);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.Size = new System.Drawing.Size(256, 174);
            this.dataGridView.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(193, 294);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // data
            // 
            this.data.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.data.AutoSize = true;
            this.data.Location = new System.Drawing.Point(203, 19);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(65, 13);
            this.data.TabIndex = 3;
            this.data.Text = "00/00/0000";
            // 
            // matricula
            // 
            this.matricula.AutoSize = true;
            this.matricula.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matricula.Location = new System.Drawing.Point(13, 50);
            this.matricula.Name = "matricula";
            this.matricula.Size = new System.Drawing.Size(42, 13);
            this.matricula.TabIndex = 4;
            this.matricula.Text = "01234";
            // 
            // nome
            // 
            this.nome.AutoSize = true;
            this.nome.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nome.Location = new System.Drawing.Point(61, 45);
            this.nome.Name = "nome";
            this.nome.Size = new System.Drawing.Size(177, 18);
            this.nome.TabIndex = 5;
            this.nome.Text = "NOME FUNCIONÁRIO";
            // 
            // titulo
            // 
            this.titulo.AutoSize = true;
            this.titulo.Location = new System.Drawing.Point(12, 19);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(110, 13);
            this.titulo.TabIndex = 6;
            this.titulo.Text = "Demonstrativo do dia:";
            // 
            // labelSemBatidasNoDia
            // 
            this.labelSemBatidasNoDia.AutoSize = true;
            this.labelSemBatidasNoDia.Location = new System.Drawing.Point(20, 80);
            this.labelSemBatidasNoDia.Name = "labelSemBatidasNoDia";
            this.labelSemBatidasNoDia.Size = new System.Drawing.Size(0, 13);
            this.labelSemBatidasNoDia.TabIndex = 7;
            // 
            // horaCerta
            // 
            this.horaCerta.AutoSize = true;
            this.horaCerta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.horaCerta.Location = new System.Drawing.Point(12, 262);
            this.horaCerta.Name = "horaCerta";
            this.horaCerta.Size = new System.Drawing.Size(159, 20);
            this.horaCerta.TabIndex = 8;
            this.horaCerta.Text = "Hora certa: 00:00h";
            this.horaCerta.Click += new System.EventHandler(this.horaCerta_Click);
            // 
            // DemonstrativoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 329);
            this.Controls.Add(this.horaCerta);
            this.Controls.Add(this.labelSemBatidasNoDia);
            this.Controls.Add(this.titulo);
            this.Controls.Add(this.nome);
            this.Controls.Add(this.matricula);
            this.Controls.Add(this.data);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 310);
            this.Name = "DemonstrativoForm";
            this.ShowInTaskbar = false;
            this.Text = "REP Virtual";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label data;
        private System.Windows.Forms.Label matricula;
        private System.Windows.Forms.Label nome;
        private System.Windows.Forms.Label titulo;
        private System.Windows.Forms.Label labelSemBatidasNoDia;
        private System.Windows.Forms.Label horaCerta;
    }
}