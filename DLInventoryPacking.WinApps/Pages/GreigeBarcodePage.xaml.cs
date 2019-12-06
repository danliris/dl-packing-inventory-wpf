using DLInventoryPacking.WinApps.Jobs;
using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DLInventoryPacking.WinApps.Pages
{
    /// <summary>
    /// Interaction logic for GreigeBarcodePage.xaml
    /// </summary>
    public partial class GreigeBarcodePage : Page
    {
        private List<BarcodeInfo> _barcodes;

        public GreigeBarcodePage()
        {
            InitializeComponent();
            _barcodes = new List<BarcodeInfo>();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (BarcodeListView.Items.Count > 0)
            {
                var printBarcodeJob = new BarcodePrintJob();
                printBarcodeJob.PrintBarcode(_barcodes);
            }
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = "Harap isi semua kolom berikut: \n";
            var anyError = false;
            if (string.IsNullOrWhiteSpace(WovenTypeTextBox.Text) || string.IsNullOrWhiteSpace(WovenTypeTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Komposisi\n";
            }

            if (string.IsNullOrWhiteSpace(Construction1TextBox.Text) || string.IsNullOrWhiteSpace(Construction2TextBox.Text))
            {
                anyError = true;
                errorMessage += "- Konstruksi\n";
            }

            if (string.IsNullOrWhiteSpace(WidthTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Lebar\n";
            }

            if (string.IsNullOrWhiteSpace(YarnType1PrefixTextBox.Text) || string.IsNullOrWhiteSpace(YarnType1SuffixTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Jenis Benang 1\n";
            }

            if (string.IsNullOrWhiteSpace(YarnType2PrefixTextBox.Text) || string.IsNullOrWhiteSpace(YarnType2SuffixTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Jenis Benang 2\n";
            }

            if (string.IsNullOrWhiteSpace(GradeTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Grade\n";
            }

            decimal.TryParse(QuantityTextBox.Text, out var quantity);
            if (string.IsNullOrWhiteSpace(QuantityTextBox.Text) || quantity <= 0)
            {
                anyError = true;
                errorMessage += "- Quantity\n";
            }

            if (anyError)
            {
                MessageBox.Show(errorMessage);
            }
            else
            {
                var viewModel = new ProductViewModel(
                    string.Empty,
                    $"{Construction1TextBox.Text} X {Construction2TextBox.Text}",
                    string.Empty,
                    GradeTextBox.Text,
                    string.Empty,
                    "GREIGE",
                    WidthTextBox.Text,
                    WovenTypeTextBox.Text,
                    $"{YarnType1PrefixTextBox.Text}{YarnType1SuffixTextBox.Text}",
                    $"{YarnType2PrefixTextBox.Text}{YarnType2SuffixTextBox.Text}",
                    "MTR",
                    quantity,
                    "PCS"
                    );
                var barcode = await PackingInventoryService.PostProduct(viewModel);

                if (barcode != null && !string.IsNullOrWhiteSpace(barcode.Code))
                {
                    Construction1TextBox.Text = string.Empty;
                    Construction2TextBox.Text = string.Empty;
                    WidthTextBox.Text = string.Empty;
                    GradeTextBox.Text = string.Empty;
                    WovenTypeTextBox.Text = string.Empty;
                    YarnType1PrefixTextBox.Text = string.Empty;
                    YarnType1SuffixTextBox.Text = string.Empty;
                    YarnType2PrefixTextBox.Text = string.Empty;
                    YarnType2SuffixTextBox.Text = string.Empty;
                    QuantityTextBox.Text = string.Empty;

                    _barcodes.Add(new BarcodeInfo() { Code = barcode.Code, PackType = barcode.PackType, Quantity = barcode.Quantity, SKUId = barcode.SKUId, UOMUnit = barcode.UOMUnit });
                    BarcodeListView.Items.Add(new BarcodeInfo() { Code = barcode.Code, PackType = barcode.PackType, Quantity = barcode.Quantity, SKUId = barcode.SKUId, UOMUnit = barcode.UOMUnit });
                }
                MessageBox.Show("data berhasil disimpan");
            }
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
