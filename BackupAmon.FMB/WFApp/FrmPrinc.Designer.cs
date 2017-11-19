using System;
using System.Net.Http;
using Amon.Nucleo.Utils;
using Amon.PontoE.Modelo.Cadastro;
using Newtonsoft.Json;

namespace WFApp
{
    partial class FrmPrinc
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrinc));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerObservador = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
         
            // timer
            this.timer.Enabled = true;
            this.timer.Interval = 40000;
            this.timer.Tick += this.timer_Tick;

            // timerObservador: Criado para verificar a execução do WFReboot.exe
            this.timerObservador.Interval = 1000;
            this.timerObservador.Tick += this.timerObservador_Tick;

            // FrmPrinc
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(142, 24);
            this.ControlBox = false;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPrinc";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.Text = "REP Virtual";
            this.TopMost = true;
            this.Load += FrmPrinc_Load;
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerObservador;
    }
}