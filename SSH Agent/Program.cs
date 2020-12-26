using HelloSSH.Agent;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using HelloSSH.KeyManager;

namespace HelloSSH
{
    class Program
    {
        const string DefaultConfigLocation = "helossh.json";
        [STAThread]
        static void Main(string[] args)
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
            var agent = new HelloSSHAgent(new ConfigurationProvider(configLocation));
            Thread agentThread = new Thread(agent.ListenOnNamedPipe);
            agentThread.IsBackground = true;
            agentThread.Start();
            TrayIcon.CreateTrayIcon();
            Application.Run();
        }

        private static void PrintUsageAndExit()
        {
            MessageBox.Show(null, @"Usage: heloossh.exe [C:\path\to\config.json]", "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(-1);
        }
    }
}
