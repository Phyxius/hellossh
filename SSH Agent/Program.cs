using HelloSSH.Agent;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using HelloSSH.KeyManager;
using HelloSSH.DataStore;
using Windows.Security.Credentials;

namespace HelloSSH
{
    partial class Program : System.Windows.Application
    {
        const string DefaultConfigLocation = "helossh.json";

        private static void PrintUsageAndExit()
        {
            MessageBox.Show(null, @"Usage: heloossh.exe [C:\path\to\config.json]", "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        public async void OnApplicationStart(object sender, System.Windows.StartupEventArgs e)
        {
            if (e.Args.Length > 1)
            {
                PrintUsageAndExit();
                return;
            }
            var configLocation = DefaultConfigLocation;
            if (e.Args.Length > 0)
            {
                configLocation = e.Args[1];
            }
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!(await KeyCredentialManager.IsSupportedAsync()))
            {
                var page = new TaskDialogPage
                {
                    Heading = "Windows Hello is Unavailable",
                    Icon = TaskDialogIcon.Error,
                    Text = "Windows Hello appears to be unavailable. This is probably because you haven't enabled it. Would you like help enabling it?",
                    Buttons =
                    {
                        TaskDialogButton.Yes,
                        TaskDialogButton.No
                    }
                };
                if (TaskDialog.ShowDialog(page) == TaskDialogButton.Yes)
                {
                    Util.OpenURI(new Uri("https://support.microsoft.com/en-us/windows/learn-about-windows-hello-and-set-it-up-dae28983-8242-bb2a-d3d1-87c9d265a5f0"));
                }
                Application.Exit();
                return;
            }
            var dataStore = new SynchronizedDataStore(new ConfigurationProvider(configLocation));
            var agent = new HelloSSHAgent(dataStore);
            Thread agentThread = new Thread(agent.ListenOnNamedPipe)
            {
                IsBackground = true
            };
            agentThread.Start();
            TrayIcon.CreateTrayIcon(dataStore);
        }
    }
}
