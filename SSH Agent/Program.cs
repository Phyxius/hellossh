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
        private static Mutex singleInstanceMutex; 
        private void PrintUsageAndExit()
        {
            MessageBox.Show(null, @"Usage: heloossh.exe [C:\path\to\config.json]", "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Shutdown();
        }

        private void ShowNoHelloErrorAndExit()
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
            Shutdown();
        }

        private void ShowInstanceAlreadyRunningErrorAndExit()
        {
            TaskDialog.ShowDialog(new TaskDialogPage
            {
                Icon = TaskDialogIcon.Error,
                Heading = "HelloSSH is already running",
                Text = "Only one instance of the application can run at a time.",
                Caption = "HelloSSH is already running",
                Buttons =
                {
                    new TaskDialogButton("Exit")
                }
            });
            Shutdown();
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
            singleInstanceMutex = new Mutex(true, "{CE870DAD-FE6D-4C89-A42A-B3D4294205CA}", out bool isNew);
            if (!isNew)
            {
                ShowInstanceAlreadyRunningErrorAndExit();
                return;
            }
            if (!await KeyCredentialManager.IsSupportedAsync())
            {
                ShowNoHelloErrorAndExit();
                return;
            }
            var dataStore = new SynchronizedDataStore(new ConfigurationProvider(configLocation, out var defaultConfigCreated));
            var agent = new HelloSSHAgent(dataStore);
            if (defaultConfigCreated)
            {
                ShowWelcome(dataStore);
            }
            Thread agentThread = new Thread(agent.ListenOnNamedPipe)
            {
                IsBackground = true
            };
            agentThread.Start();
            TrayIcon.CreateTrayIcon(dataStore);
            agent.PrivateKeyRequested += TrayIcon.NotifyKeyUsed;
        }

        private void ShowWelcome(SynchronizedDataStore dataStore)
        {
            var yesButton = new TaskDialogCommandLinkButton("Yes", $"A key with the name \"{Configuration.DEFAULT_KEY_NAME}\" will be created. You will be asked to confirm your identity with Windows Hello.");
            var choice = TaskDialog.ShowDialog(new TaskDialogPage
            {
                Icon = TaskDialogIcon.Information,
                Caption = "Welcome to HelloSSH",
                Heading = "Welcome",
                Text = "It looks like this is your first time running HelloSSH. Would you like to create a key with the default settings?",
                Buttons =
                {
                    yesButton,
                    new TaskDialogCommandLinkButton("No", "No keys will be created. You can create one later by double-clicking on the HelloSSH icon in the notification tray.")
                }
            });
            if (choice == yesButton)
            {
                var keyCreated = false;
                while (!(keyCreated = dataStore.AddKey(Configuration.DEFAULT_KEY_NAME))
                    && (TaskDialog.ShowDialog(new TaskDialogPage
                    {
                        Icon = TaskDialogIcon.Error,
                        Caption = "Error creating key",
                        Heading = "Error creating key",
                        Text = "There was an error creating the key. This might have been because you pressed Cancel on the Windows Hello dialog, or because of a system error. Would you like to try again?",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        }
                    }) == TaskDialogButton.Yes)) ;
                if (keyCreated)
                {
                    TaskDialog.ShowDialog(new TaskDialogPage
                    {
                        Caption = "Successfully created key",
                        Heading = "Success",
                        Text = "The key was successfully created. You can now use it as an SSH key. If you haven't already, see the README for more information on how to use HelloSSH."
                    });
                }
            }
        }
    }
}
