using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DLInventoryPacking.WinApps.Pages
{
    /// <summary>
    /// Interaction logic for GreigeBarcodePage.xaml
    /// </summary>
    public partial class GreigeBarcodePage : Page
    {
        private List<BarcodeInfo> _barcodes;
        private SQLiteConnection _dbConnection;
        private string _dbPath = Environment.CurrentDirectory + "\\DB";
        private readonly IPConfigurationManager _dbSQLite;

        public GreigeBarcodePage()
        {
            InitializeComponent();
            _barcodes = new List<BarcodeInfo>();
            pb.Visibility = Visibility.Hidden;
            EditButton.IsEnabled = true;
            TestButton.IsEnabled = false;
            SaveButton.IsEnabled = false;

            IPTextBox.IsReadOnly = true;

            _dbSQLite = new IPConfigurationManager();

            IPTextBox.Text = _dbSQLite.GetIP();
            IPTextBox.IsReadOnly = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            IPTextBox.IsReadOnly = false;
            SaveButton.IsEnabled = true;
            EditButton.IsEnabled = false;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _dbSQLite.UpdateIP(IPTextBox.Text);
            EditButton.IsEnabled = true;
            SaveButton.IsEnabled = false;
        }
    }
}
