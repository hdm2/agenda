using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFReboot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.ObservaProcessos();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("xxxx");
        }
    }
}
