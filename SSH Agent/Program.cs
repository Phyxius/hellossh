using HelloSSH.Agent;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

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
            new Thread(agent.ListenOnNamedPipe).Start();
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("&Exit");
            contextMenu.Items.Add("&About HelloSSH");
            contextMenu.Items.Add("Open key &manager");
            var icon = new NotifyIcon
            {
                Icon = Properties.Resources.ApplicationIcon,
                Visible = true,
                Text = "HelloSSH",
                ContextMenuStrip = contextMenu
            };
            Application.Run();
        }

        private static void PrintUsageAndExit()
        {
            Console.WriteLine(@"Usage: heloossh.exe [C:\path\to\config.json]");
            Environment.Exit(-1);
        }
    }
}
