using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloSSH.KeyManager
{
    internal static class TrayIcon
    {
        private static NotifyIcon icon;
        public static void CreateTrayIcon()
        {
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
        }

        private static void ExitButtonClick(object sender, EventArgs e)
        {
            icon.Visible = false;
            Application.Exit();
        }
        private static void AboutButtonClick(object sender, EventArgs e)
        {
            AboutBox.Show();
        }
        private static void ManageButtonClick(object sender, EventArgs e)
        {

        }
    }
}
