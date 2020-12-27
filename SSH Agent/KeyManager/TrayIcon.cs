using HelloSSH.DataStore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloSSH.KeyManager
{
    internal static class TrayIcon
    {
        private static NotifyIcon icon;
        private static SynchronizedDataStore dataStore;
        public static void CreateTrayIcon(SynchronizedDataStore dataStore)
        {
            TrayIcon.dataStore = dataStore;
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open key &manager").Click += ManageButtonClick;
            contextMenu.Items.Add("&About HelloSSH").Click += AboutButtonClick;
            contextMenu.Items.Add("&Exit").Click += ExitButtonClick;
            icon = new NotifyIcon
            {
                Icon = Properties.Resources.ApplicationIcon,
                Visible = true,
                Text = "HelloSSH",
                ContextMenuStrip = contextMenu
            };
            icon.DoubleClick += ManageButtonClick;
        }

        public static void NotifyKeyUsed(HelloSSHKey key, uint clientProcessId)
        {
            var clientProcess = Process.GetProcessById((int)clientProcessId);
            icon.ShowBalloonTip(3000, "Private Key Requested", $"{clientProcess.ProcessName} wants to sign a challenge with key {key.Comment}.", ToolTipIcon.Info);
        }
        private static void ExitButtonClick(object sender, EventArgs e)
        {
            icon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }
        private static void AboutButtonClick(object sender, EventArgs e)
        {
            AboutBox.Show();
        }
        private static void ManageButtonClick(object sender, EventArgs e)
        {
            KeyManager.Show(dataStore);
        }

    }
}
