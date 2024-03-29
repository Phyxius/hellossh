﻿using HelloSSH.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HelloSSH.KeyManager
{
    /// <summary>
    /// Interaction logic for KeyManager.xaml
    /// </summary>
    public partial class KeyManager : Window
    {
        private static KeyManager keyManager;
        private readonly SynchronizedDataStore dataStore;
        internal KeyManager(SynchronizedDataStore dataStore)
        {
            this.dataStore = dataStore;
            InitializeComponent();
            KeysList.ItemsSource = dataStore.Keys;
        }

        protected override void OnClosed(EventArgs e)
        {
            keyManager = null;
            base.OnClosed(e);
        }

        internal static void Show(SynchronizedDataStore dataStore)
        {
            if (keyManager != null)
            {
                keyManager.Activate();
                return;
            }

            keyManager = new KeyManager(dataStore);
            keyManager.Show();
        }

        private void NewKey_Click(object sender, RoutedEventArgs e)
        {
            var newKeyName = 
                AddKeyDialog.ShowCreateKeyDialog(this, dataStore.Keys.Select(k => k.Comment));
            if (newKeyName == null)
            {
                return;
            }

            dataStore.AddKey(newKeyName);
        }

        private void RemoveKey_Click(object sender, RoutedEventArgs e)
        {
            var keyName = ((HelloSSHKey)KeysList.SelectedItem).Comment;
            var yesButton = new TaskDialogButton("Delete");
            var page = new TaskDialogPage
            {
                Caption = "Confirmation",
                Icon = TaskDialogIcon.Warning,
                Heading = "Are you sure?",
                Text = $"Are you sure you want to delete the key \"{keyName}? This action is irreversible.",
                Buttons =
                {
                    TaskDialogButton.Cancel, yesButton
                }
            };
            if (TaskDialog.ShowDialog(new WindowInteropHelper(this).Handle, page) != yesButton)
            {
                return;
            }
            dataStore.RemoveKey(keyName);
        }
        
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            var menu = FindResource("CopyContextMenu") as ContextMenu;
            menu.DataContext = KeysList;
            menu.PlacementTarget = (UIElement)sender;
            menu.IsOpen = true;
        }
        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText((KeysList.SelectedItem as HelloSSHKey).PublicKeyHash);
        }

        private void CopyFingerprint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText((KeysList.SelectedItem as HelloSSHKey).PublicKeyFingerprint);
        }
        private void CopyAuthorizedKeys_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(dataStore.GetAuthorizedKeysFile());
        }

        private void CopyAttestation_Click(object sender, RoutedEventArgs e)
        {
            var key = KeysList.SelectedItem as HelloSSHKey;
            var status = key.GetAttestation(out var result);
            if (status != Windows.Security.Credentials.KeyCredentialAttestationStatus.Success)
            {
                TaskDialog.ShowDialog(new TaskDialogPage
                {
                    Caption = "Error retrieving attestation",
                    Heading = "Couldn't retrieve the attestation",
                    Icon = TaskDialogIcon.Error,
                    Text = $"Retrieving the attestation for the key {key.Comment} failed. The result returned by the system was \"{status}\"."
                });
                return;
            }

            System.Windows.Clipboard.SetText(result.Serialize());
        }
    }
}
