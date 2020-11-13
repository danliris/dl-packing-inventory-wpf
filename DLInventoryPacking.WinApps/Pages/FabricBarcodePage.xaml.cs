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
    /// Interaction logic for FabricBarcodePage.xaml
    /// </summary>
    public partial class FabricBarcodePage : Page
    {
        private List<BarcodeInfo> _barcodes;

        public FabricBarcodePage()
        {
            InitializeComponent();
            _barcodes = new List<BarcodeInfo>();
            pb.Visibility = Visibility.Hidden;
        }

        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            FormGrid.IsEnabled = false;
            //pb.Visibility = Visibility.Visible;
            var errorMessage = "Harap isi semua kolom berikut: \n";
            var anyError = false;
            if (string.IsNullOrWhiteSpace(CompositionTextBox.Text) || string.IsNullOrWhiteSpace(CompositionTextBox.Text))
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

            if (string.IsNullOrWhiteSpace(DesignTextBox.Text))
            {
                anyError = true;
                errorMessage += "- Design\n";
            }

            if (string.IsNullOrWhiteSpace(GradeComboBox.Text))
            {
                anyError = true;
                errorMessage += "- Grade\n";
            }

            //decimal.TryParse(QuantityTextBox.Text, out var quantity);
            var quantity = QuantityDecimalUpDown.Value.GetValueOrDefault();
            if (quantity <= 0)
            {
                anyError = true;
                errorMessage += "- Quantity\n";
            }

            if (string.IsNullOrWhiteSpace(PackTypeComboBox.Text))
            {
                anyError = true;
                errorMessage += "- Jenis Pack\n";
            }

            if (anyError)
            {
                //FormGrid.IsEnabled = true;
                //pb.Visibility = Visibility.Hidden;
                MessageBox.Show(errorMessage);
            }
            else
            {
                pb.Visibility = Visibility.Visible;
                var viewModel = new ProductViewModel(
                    CompositionTextBox.Text,
                    $"{Construction1TextBox.Text} X {Construction2TextBox.Text}",
                    DesignTextBox.Text,
                    GradeComboBox.Text,
                    string.Empty,
                    "FABRIC",
                    WidthTextBox.Text,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "MTR",
                    quantity,
                    PackTypeComboBox.Text
                    );
                //var barcode = await PackingInventoryService.PostProduct(viewModel);

                //if (barcode != null && !string.IsNullOrWhiteSpace(barcode.PackingCode))
                //{
                //    CompositionTextBox.Text = string.Empty;
                //    Construction1TextBox.Text = string.Empty;
                //    Construction2TextBox.Text = string.Empty;
                //    DesignTextBox.Text = string.Empty;
                //    GradeComboBox.Text = string.Empty;
                //    WidthTextBox.Text = string.Empty;
                //    PackTypeComboBox.Text = string.Empty;
                //    QuantityDecimalUpDown.Value = null;

                //    _barcodes.Add(barcode); ;
                //    BarcodeListView.Items.Add(barcode);
                //}

                //FormGrid.IsEnabled = true;
                //pb.Visibility = Visibility.Hidden;
                MessageBox.Show("data berhasil disimpan");
                pb.Visibility = Visibility.Hidden;
            }

            FormGrid.IsEnabled = true;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            FormGrid.IsEnabled = false;
            if (BarcodeListView.Items.Count > 0)
            {
                var printBarcodeJob = new BarcodePrintJob();
                //printBarcodeJob.PrintBartenderJob(_barcodes);
            }
            FormGrid.IsEnabled = true;
        }
    }
}
