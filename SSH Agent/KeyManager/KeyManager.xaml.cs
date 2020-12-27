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
using System.Windows.Input;
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
        private readonly SynchronizedDataStore dataStore;
        internal KeyManager(SynchronizedDataStore dataStore)
        {
            this.dataStore = dataStore;
            InitializeComponent();
            KeysList.ItemsSource = dataStore.Keys;
        }
    }
}