using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HelloSSH.KeyManager
{
    /// <summary>
    /// Interaction logic for AddKeyDialog.xaml
    /// </summary>
    public partial class AddKeyDialog : Window
    {
        bool canceled = true;
        HashSet<string> existingKeyNames;
        public AddKeyDialog()
        {
            InitializeComponent();
        }

        void Cancel(object sender, EventArgs e)
        {
            Close();
        }

        void CreateKey(object sender, EventArgs e)
        {
            canceled = false;
            Close();
        }

        public static string ShowCreateKeyDialog(Window owner, IEnumerable<string> existingKeyNames)
        {
            var dialog = new AddKeyDialog
            {
                Owner = owner,
                existingKeyNames = new HashSet<string>(existingKeyNames)
            };
            dialog.existingKeyNames.Add("");
            dialog.ShowDialog();
            if (dialog.canceled)
            {
                return null;
            }

            return dialog.KeyNameInput.Text.Trim();
        }

        private void KeyNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateKeyButton.IsEnabled = !existingKeyNames.Contains(KeyNameInput.Text.Trim());
        }
    }
}
