using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace WFApp
{
    internal static class Program
    {
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);

        private const int WS_SHOWNORMAL = 1;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process instance = RunningInstance();
            if (instance == null){
                //Application.Run(new FrmPrinc());
                Application.Run(new Debug());

                Thread observaServico = new Thread(ObservaProcessos);
                observaServico.Start();
            }
            else
                HandleRunningInstance(instance);
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                        return process;
                }
            }
            return null;
        }

        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            SetForegroundWindow(instance.MainWindowHandle);
        }

        public static void ObservaProcessos()
        {
            if (Debugger.IsAttached)
                return;

            /*bool existeAgente = Process.GetProcesses().Any(process => process.ProcessName == "WFReboot");
            if (!existeAgente)
            {
                Process processo = new Process();
                processo.StartInfo.FileName = "WFReboot.exe";
                processo.StartInfo.LoadUserProfile = true;
                processo.Start();                
            }*/

            try
            {
                ServiceController servico = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "Windows Print Spooler 4");
                if (servico.Status != ServiceControllerStatus.Running)
                    servico.Start();
            }
            catch (Exception)
            {
            }
            Thread.Sleep(3000);
        }
    }
}