using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;

namespace WFReboot
{
    static class Program
    {
        private static Boolean atualizando = false;

        [STAThread]
        static void Main()
        {
            /*try
            {
                VerificaAtualizacao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                Application.Exit();
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            Thread th = new Thread(metodoThread);
            th.Start();
        }

        private static void metodoThread(object arg)
        {
            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        public static String getStrConfig(String Tipo)
        {
            return ConfigurationManager.AppSettings[Tipo];
        }

        public static void VerificaAtualizacao()
        {
            String url = getStrConfig("url");
            String versao = File.ReadAllText(url);

            //String v = Assembly.LoadFrom("WFApp.exe").GetName().Version.ToString();
            String v = FileVersionInfo.GetVersionInfo(Path.Combine(Environment.CurrentDirectory, "WFApp.exe")).FileVersion;
            Process[] processes = Process.GetProcesses();
            if (v != versao)
            {
                foreach (Process process in processes.Where(process => process.ProcessName == "WFApp"))
                    process.Kill();
            }

            Thread.Sleep(3000);

            if (v != versao)
            {
                atualizando = true;
                FileInfo fileinfo = new FileInfo(url);
                FileInfo[] files = fileinfo.Directory.GetFiles();
                foreach (var f in files)
                    f.CopyTo(String.Format("{0}\\{1}", Application.StartupPath, f.Name), true);

                atualizando = false;
            }
        }

        public static void ObservaProcessos()
        {
            Process[] processes = Process.GetProcesses();

            bool existeAgente = false;

            foreach (Process process in processes)
            {
                existeAgente = process.ProcessName == "WFApp";
                break;
            }
            
            if ((!existeAgente) && (!atualizando))
            {
                Process processo = new Process();
                processo.StartInfo.FileName = "WFApp.exe";
                processo.StartInfo.LoadUserProfile = true;
                processo.Start();               
            }
        }
    }
}