﻿using DLInventoryPacking.WinApps.Jobs;
using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DLInventoryPacking.WinApps.Pages
{
    /// <summary>
    /// Interaction logic for YarnBarcode.xaml
    /// </summary>
    public partial class YarnBarcodePage : Page
    {
        private List<BarcodeInfo> _barcodes;

        public YarnBarcodePage()
        {
            InitializeComponent();
            _barcodes = new List<BarcodeInfo>();
            pb.Visibility = Visibility.Hidden;
            DeleteButton.IsEnabled = false;
        }

        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            FormGrid.IsEnabled = false;
            var errorMessage = "Harap isi semua kolom berikut: \n";
            var anyError = false;
            if (string.IsNullOrWhiteSpace(YarnTypePrefixTextBox.Text) || string.IsNullOrWhiteSpace(YarnTypeSuffixTextbox.Text))
            {
                anyError = true;
                errorMessage += "- Jenis Benang\n";
            }

            if (string.IsNullOrWhiteSpace(LotNoTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Nomor Lot\n";
            }

            //decimal.TryParse(QuantityTextBox.Text, out var quantity);
            //if (string.IsNullOrWhiteSpace(QuantityTextBox.Text) || quantity <= 0)
            //{
            //    anyError = true;
            //    errorMessage += "- Quantity\n";
            //}

            //var quantity = QuantityDecimalUpDown.Value.GetValueOrDefault();

            if (anyError)
            {
                MessageBox.Show(errorMessage);
            }
            else
            {
                pb.Visibility = Visibility.Visible;
                var viewModel = new ProductViewModel(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    LotNoTextBox.Text,
                    "YARN",
                    string.Empty,
                    string.Empty,
                    YarnTypePrefixTextBox.Text + YarnTypeSuffixTextbox.Text,
                    string.Empty,
                    "BALE",
                    QuantityDecimalUpDown.Value.GetValueOrDefault(),
                    "PALET"
                    );
                var barcode = await PackingInventoryService.PostProduct(viewModel);

                //if (barcode != null && !string.IsNullOrWhiteSpace(barcode.PackingCode))
                if (barcode != null /*&& !string.IsNullOrWhiteSpace(barcode.PackingCode)*/)
                    {
                    LotNoTextBox.Text = string.Empty;
                    YarnTypePrefixTextBox.Text = string.Empty;
                    YarnTypeSuffixTextbox.Text = string.Empty;
                    //QuantityTextBox.Text = string.Empty;
                    QuantityDecimalUpDown.Value = null;

                    _barcodes.Add(barcode);
                    BarcodeListView.Items.Add(barcode);
                }
                MessageBox.Show("data berhasil disimpan");
                pb.Visibility = Visibility.Hidden;
            }
            FormGrid.IsEnabled = true;

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (BarcodeListView.Items.Count > 0)
            {
                var printBarcodeJob = new BarcodePrintJob();
                printBarcodeJob.PrintBartenderJob(_barcodes);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (BarcodeListView.Items.)
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem listViewItem in BarcodeListView.SelectedItems)
            {
                BarcodeListView.Items.Remove(listViewItem);
            }
        }

        private void BarcodeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BarcodeListView.SelectedItems.Count > 0)
                DeleteButton.IsEnabled = true;
        }
    }
}
