﻿using DLInventoryPacking.WinApps.Jobs;
using DLInventoryPacking.WinApps.Pages;
using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLInventoryPacking.WinApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YarnBarcodePage Yarnpage = new YarnBarcodePage();
        private FabricBarcodePage Fabricpage = new FabricBarcodePage();
        private GreigeBarcodePage Greigepage = new GreigeBarcodePage();
        public MainWindow()
        {
            InitializeComponent();
            MenuGrid.Visibility = Visibility.Hidden;
            LoginGrid.Visibility = Visibility.Visible;

            Button btn = new Button();
            btn.Name = "YarnBarcodeButton";
            btn.Click += YarnBarcodeButton_Click;

        }



        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("Username atau Password harus diisi!");
            }
            else
            {
                UserCredentials.Token = await AuthService.Authenticate(UsernameTextBox.Text, PasswordTextBox.Password);

                if (string.IsNullOrWhiteSpace(UserCredentials.Token))
                {
                    MessageBox.Show("Username atau Password salah!");
                }
                else
                {
                    LoginGrid.Visibility = Visibility.Hidden;
                    MenuGrid.Visibility = Visibility.Visible;
                    _NavigationFrame.Navigate(new HomePage());
                    
                }
                Mouse.OverrideCursor = null;
            }

            
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {

            string content = (sender as Button).Content.ToString();
            _NavigationFrame.Navigate(new HomePage());
        }

        private void YarnBarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            //var validate = new Validate();
            //validate.jumlah();
            
            if (Fabricpage.BarcodeListView.Items.Count != 0 )
            {
                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else if (Greigepage.BarcodeListView.Items.Count != 0)
            {
                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else
            {
                string content = (sender as Button).Content.ToString();
                _NavigationFrame.Navigate(new YarnBarcodePage());

            }
        }

        private void FabricBarcodeButton_Click(object sender, RoutedEventArgs e)
        {

         

            if (Yarnpage.BarcodeListView.Items.Count > 0)
            {

                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else if (Greigepage.BarcodeListView.Items.Count > 0)
            {

                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else
            {
                _NavigationFrame.Navigate(new FabricBarcodePage());

            }
            //_NavigationFrame.Navigate(new FabricBarcodePage());
        }

        private void GreigeBarcodeButton_Click(object sender, RoutedEventArgs e)
        {

            if (Fabricpage.BarcodeListView.Items.Count >0)
            {
                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else if (Greigepage.BarcodeListView.Items.Count >0)
            {
                MessageBox.Show("Apakah Anda Tidak Ingin Mencetak Barcode?");
            }
            else
            {
                _NavigationFrame.Navigate(new GreigeBarcodePage());

            }
            // _NavigationFrame.Navigate(new GreigeBarcodePage());
        }
    }
}
