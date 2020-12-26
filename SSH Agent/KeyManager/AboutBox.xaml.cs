using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloSSH.KeyManager
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
            Icon = SystemIcons.Information.ToImageSource();
        }

        private void OpenHyperlink(object sender, RequestNavigateEventArgs e)
        {
            Util.OpenURI(e.Uri);
        }

        public static void Show(Window owner = null)
        {
            new AboutBox
            {
                Owner = owner
            }.ShowDialog();
        }
    }
}
