using DLInventoryPacking.WinApps.Jobs;
using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //PrintButton.IsEnabled = false;
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
            if (string.IsNullOrWhiteSpace(OrderNo.Text) || string.IsNullOrWhiteSpace(OrderNo.Text))
            {
                anyError = true;
                errorMessage += "- Nomor Order\n";
            }

            //if (string.IsNullOrWhiteSpace(LotNoTextBox.Text))
            //{
            //    anyError = true;
            //    errorMessage += "- Nomor Lot\n";
            //}

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
                //var viewModel = new ProductViewModel(
                //    string.Empty,
                //    string.Empty,
                //    string.Empty,
                //    string.Empty,
                //    LotNoTextBox.Text,
                //    "YARN",
                //    string.Empty,
                //    string.Empty,
                //    YarnTypePrefixTextBox.Text + YarnTypeSuffixTextbox.Text,
                //    string.Empty,
                //    "BALE",
                //    QuantityDecimalUpDown.Value.GetValueOrDefault(),
                //    "PALET"
                //    );
                var barcodeList = await PackingInventoryService.GetBarcodeInfoByOrderNo(OrderNo.Text);

                _barcodes = new List<BarcodeInfo>();
                BarcodeListView.Items.Clear();
                foreach (var barcode in barcodeList)
                {
                    foreach (var packingCode in barcode.productPackingCodes)
                    {
                        var barcodeInfo = new BarcodeInfo()
                        {
                            Color = barcode.color,
                            MaterialConstructionName = barcode.materialConstruction.name,
                            MaterialName = barcode.material.name,
                            OrderNo = barcode.productionOrder.no,
                            PackingCode = packingCode,
                            PackingLength = barcode.productPackingLength.ToString(),
                            PackingType = barcode.productPackingType,
                            YarnMaterialName = barcode.yarnMaterial.name,
                            UOMSKU = barcode.uomUnit
                        };
                        _barcodes.Add(barcodeInfo);
                        BarcodeListView.Items.Add(barcodeInfo);
                    }
                }


                ////if (barcode != null && !string.IsNullOrWhiteSpace(barcode.PackingCode))
                //if (barcode != null /*&& !string.IsNullOrWhiteSpace(barcode.PackingCode)*/)
                //    {
                //    LotNoTextBox.Text = string.Empty;
                //    YarnTypePrefixTextBox.Text = string.Empty;
                //    YarnTypeSuffixTextbox.Text = string.Empty;
                //    //QuantityTextBox.Text = string.Empty;
                //    QuantityDecimalUpDown.Value = null;

                //    _barcodes.Add(barcode);
                //    BarcodeListView.Items.Add(barcode);
                //}

                pb.Visibility = Visibility.Hidden;
            }
            FormGrid.IsEnabled = true;

            //MessageBox.Show()

        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //if (BarcodeListView.Items.Count > 0)
            //{
            //    var printBarcodeJob = new BarcodePrintJob();
            //    printBarcodeJob.PrintBartenderJob(_barcodes);
            //}

            var zplString = "";
            var barcodes = BarcodeListView.Items.Cast<BarcodeInfo>().ToList();
            foreach (var barcode in barcodes)
            {
                zplString += $"^XA^MMT^PW382^LL0635^LS0^CFB,15,7^FO20,250^FDAnyaman^FS^FO150,250^FD{barcode.MaterialName}^FS^FO20,275^FDKonstruksi^FS^FO150,275^FD{barcode.MaterialConstructionName}^FS^FO250,275^FD{barcode.YarnMaterialName}^FS^FO20,300^FDPanjang^FS^FO150,300^FD{barcode.PackingLength}^FS^FO250,300^FD{barcode.UOMSKU}^FS^FO20,325^FDMotif/Warna^FS^FO150,325^FD{barcode.Color}^FS^FO20,550^FD{DateTime.Now}^FS^FT131,169^BQN,2,5^FH\\^FDLA,{barcode.PackingCode}^FS^FT122,182^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
            }
            //var zpl2 = "CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNM,0^MTT^PON^PMN^LH0,0^JMA^PR3,3~SD30^JUS^LRN^CI27^PA0,1,1,0^XZ^XA^MMT^PW615^LL328^LS0^FWR^FO415,70^BQ,2,8^FDQA,FD200200002PCS^FS^FO380,80^AD,12,12^FD200200002PCS^FS^FO100,25^BY2^BC,75,Y,N,N^FD200200002^FS^FO306,65^AD,12,12^FDPC25 LOT 1^FS^FO280,65^AD,11,11^FD2 BALE^FS^FO0,25^AD,10,10^FD28/04/2020^FS^PQ1,0,1,Y^XZ";
            await ZebraPrinterHelper.Print(zplString);
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
            var selectedItems = BarcodeListView.SelectedItems.Cast<BarcodeInfo>().ToArray();
            foreach (var item in selectedItems)
            {
                BarcodeListView.Items.Remove(item);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            BarcodeListView.Items.Clear();
        }

        private void BarcodeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BarcodeListView.SelectedItems.Count > 0)
            {
                DeleteButton.IsEnabled = true;
            }
        }
    }
}
