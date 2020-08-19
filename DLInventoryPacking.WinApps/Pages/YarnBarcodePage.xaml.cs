using DLInventoryPacking.WinApps.Jobs;
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

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //if (BarcodeListView.Items.Count > 0)
            //{
            //    var printBarcodeJob = new BarcodePrintJob();
            //    printBarcodeJob.PrintBartenderJob(_barcodes);
            //}
            var zpl1 = "^XA^MMT^PW382^LL0635^LS0^CFB,15,7^FO15,250^FDAnyaman^FS^FO150,250^FDT2/1^FS^FO15,275^FDKonstruksi^FS^FO150,275^FD133X72^FS^FO250,275^FDCD40XCD40^FS^FO15,300^FDPanjang^FS^FO150,300^FD100^FS^FO250,300^FDYARD^FS^FO15,325^FDMotif/Warna^FS^FO150,325^FDCPS-01/Merah^FS^FO15,600^FD2020-07-30^FS^FT131,169^BQN,2,5^FH\\^FDLA,01F01231^FS^FT147,182^A0N,16,21^FB81,1,0,C^FH\\^FD01F01231^FS^BY2,3,74^FT21,493^BCN,,Y,N^FD01F012310001^FS^PQ1,0,1,Y^XZ";
            //var zpl2 = "CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNM,0^MTT^PON^PMN^LH0,0^JMA^PR3,3~SD30^JUS^LRN^CI27^PA0,1,1,0^XZ^XA^MMT^PW615^LL328^LS0^FWR^FO415,70^BQ,2,8^FDQA,FD200200002PCS^FS^FO380,80^AD,12,12^FD200200002PCS^FS^FO100,25^BY2^BC,75,Y,N,N^FD200200002^FS^FO306,65^AD,12,12^FDPC25 LOT 1^FS^FO280,65^AD,11,11^FD2 BALE^FS^FO0,25^AD,10,10^FD28/04/2020^FS^PQ1,0,1,Y^XZ";
            await ZebraPrinterHelper.Print(zpl1 + zpl1);
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
