using HelloSSH.Agent;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using HelloSSH.KeyManager;
using HelloSSH.DataStore;

namespace HelloSSH
{
    partial class Program : System.Windows.Application
    {
        const string DefaultConfigLocation = "helossh.json";

        [STAThread]
        static void Run(string[] args)
        {
            if (args.Length > 1)
            {
                PrintUsageAndExit();
            }
            var configLocation = DefaultConfigLocation;
            if (args.Length > 0)
            {
                configLocation = args[1];
            }
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var dataStore = new SynchronizedDataStore(new ConfigurationProvider(configLocation));
            var agent = new HelloSSHAgent(dataStore);
            Thread agentThread = new Thread(agent.ListenOnNamedPipe)
            {
                IsBackground = true
            };
            agentThread.Start();
            TrayIcon.CreateTrayIcon(dataStore);
        }

        private static void PrintUsageAndExit()
        {
            MessageBox.Show(null, @"Usage: heloossh.exe [C:\path\to\config.json]", "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(-1);
        }

        public void OnApplicationStart(object sender, System.Windows.StartupEventArgs e)
        {
            Run(e.Args);
        }
    }
}
