﻿using DLInventoryPacking.WinApps.Jobs;
using DLInventoryPacking.WinApps.Services;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IDatabase _cache;
        public ObservableCollection<BarcodeInfo> BarcodeList { get; set; }

        public FabricBarcodePage()
        {
            InitializeComponent();
            _barcodes = new List<BarcodeInfo>();
            pb.Visibility = Visibility.Hidden;
            _cache = CacheService.Connection.GetDatabase();
            BarcodeGrid.ItemsSource = BarcodeList;
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
                var barcodeList = await PackingInventoryService.GetBarcodeInfoByOrderNo(OrderNo.Text, false, true);

                _barcodes = new List<BarcodeInfo>();


                if (barcodeList != null /*&& !IsReprint.IsChecked.GetValueOrDefault()*/)
                    foreach (var barcode in barcodeList)
                    {
                        //var printedListJson = _cache.StringGet(barcode.productionOrder.no);
                        var printedList = new List<string>();

                        //if (!string.IsNullOrWhiteSpace(printedListJson))
                        //{
                        //    printedList = JsonConvert.DeserializeObject<List<string>>(printedListJson, new JsonSerializerSettings()
                        //    {
                        //        MissingMemberHandling = MissingMemberHandling.Ignore
                        //    });
                        //}

                        if (printedList != null && printedList.Count > 0)
                        {
                            barcode.productPackingCodes = barcode.productPackingCodes.Where(element => !printedList.Contains(element)).ToList();
                        }

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
                                UOMSKU = barcode.uomUnit,
                                DocumentNo = barcode.documentNo,
                                Grade = barcode.grade,
                                MaterialWidth = barcode.materialWidth
                                
                            };
                            _barcodes.Add(barcodeInfo);
                            //BarcodeList.Add(barcodeInfo);
                        }




                    }
                else if (barcodeList != null /*&& IsReprint.IsChecked.GetValueOrDefault()*/)
                {
                    foreach (var barcode in barcodeList)
                    {
                        var printedListJson = _cache.StringGet(barcode.productionOrder.no);
                        var printedList = new List<string>();

                        if (!string.IsNullOrWhiteSpace(printedListJson))
                        {
                            printedList = JsonConvert.DeserializeObject<List<string>>(printedListJson, new JsonSerializerSettings()
                            {
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            });
                        }

                        if (printedList != null && printedList.Count > 0)
                        {
                            barcode.productPackingCodes = barcode.productPackingCodes.Where(element => printedList.Contains(element)).ToList();

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
                                    UOMSKU = barcode.uomUnit,
                                    DocumentNo = barcode.documentNo,
                                    Grade = barcode.grade,
                                    MaterialWidth = barcode.materialWidth
                                };
                                _barcodes.Add(barcodeInfo);
                            }

                        
                            //BarcodeList.Add(barcodeInfo);
                        }




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

                if (!string.IsNullOrWhiteSpace(PackingSizeFilter.Text))
                {
                    if (double.TryParse(PackingSizeFilter.Text, out var packingSize))
                    {
                        _barcodes = _barcodes.Where(element => element.PackingLength == packingSize.ToString()).ToList();
                    }
                }

                if (!string.IsNullOrWhiteSpace(PackingCodeFilter.Text))
                {
                    _barcodes = _barcodes.Where(element => element.PackingCode.Contains(PackingCodeFilter.Text)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(GradeFilter.Text))
                {
                    _barcodes = _barcodes.Where(element => element.Grade.Contains(GradeFilter.Text)).ToList();
                }


                //if (!string.IsNullOrWhiteSpace(DocumentFilter.Text))
                //{
                //    _barcodes = _barcodes.Where(element => element.DocumentNo.Contains(DocumentFilter.Text)).ToList();
                //}
                BarcodeList = new ObservableCollection<BarcodeInfo>();
                foreach (var _barcode in _barcodes)
                    BarcodeList.Add(_barcode);

                BarcodeGrid.ItemsSource = BarcodeList;
                pb.Visibility = Visibility.Hidden;
            }
            FormGrid.IsEnabled = true;

            //MessageBox.Show()

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //var zplString = "";
            //var barcodes = BarcodeGrid.Items.Cast<BarcodeInfo>().ToList();
            //foreach (var barcode in barcodes)
            //{
            //    var printedListJson = _cache.StringGet(barcode.OrderNo);
            //    var printedList = new List<string>();

            //    if (!string.IsNullOrWhiteSpace(printedListJson))
            //    {
            //        printedList = JsonConvert.DeserializeObject<List<string>>(printedListJson, new JsonSerializerSettings()
            //        {
            //            MissingMemberHandling = MissingMemberHandling.Ignore
            //        });

            //        if (printedList == null)
            //            printedList = new List<string>();
            //    }

            //    printedList.Add(barcode.PackingCode);
            //    printedListJson = JsonConvert.SerializeObject(printedList);
            //    _cache.StringSet(barcode.OrderNo, printedListJson);

            //    zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
            //    //zplString += $"^XA^MMT^PW382^LL0635^LS0^CFB,18,10^FO20,250^FDAnyaman^FS^FO150,250^FD{barcode.MaterialName}^FS^FO20,275^FDKonstruksi^FS^FO150,275^FD{barcode.MaterialConstructionName}^FS^FO250,275^FD{barcode.YarnMaterialName}^FS^FO20,300^FDPanjang^FS^FO150,300^FD{barcode.PackingLength}^FS^FO250,300^FD{barcode.UOMSKU}^FS^FO20,325^FDMotif/Warna^FS^FO150,325^FD{barcode.Color}^FS^FO20,550^FD{DateTime.Now}^FS^FT131,169^BQN,2,5^FH\\^FDLA,{barcode.PackingCode}^FS^FT122,182^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
            //}
            ////var zpl2 = "CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNM,0^MTT^PON^PMN^LH0,0^JMA^PR3,3~SD30^JUS^LRN^CI27^PA0,1,1,0^XZ^XA^MMT^PW615^LL328^LS0^FWR^FO415,70^BQ,2,8^FDQA,FD200200002PCS^FS^FO380,80^AD,12,12^FD200200002PCS^FS^FO100,25^BY2^BC,75,Y,N,N^FD200200002^FS^FO306,65^AD,12,12^FDPC25 LOT 1^FS^FO280,65^AD,11,11^FD2 BALE^FS^FO0,25^AD,10,10^FD28/04/2020^FS^PQ1,0,1,Y^XZ";
            //ZebraPrinterHelper.Print(zplString);

            var zplString = "";
            var barcodes = BarcodeGrid.Items.OfType<BarcodeInfo>().ToList();
            int jml = QtyPrint.Text != "" ? Int32.Parse(QtyPrint.Text) : 1;
            foreach (var barcode in barcodes)
            {
                var printedListJson = _cache.StringGet(barcode.OrderNo);
                var printedList = new List<string>();

                if (!string.IsNullOrWhiteSpace(printedListJson))
                {
                    printedList = JsonConvert.DeserializeObject<List<string>>(printedListJson, new JsonSerializerSettings()
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });

                    if (printedList == null)
                        printedList = new List<string>();
                }

                printedList.Add(barcode.PackingCode);
                printedListJson = JsonConvert.SerializeObject(printedList);
                _cache.StringSet(barcode.OrderNo, printedListJson);
                double convYard = Math.Round(Convert.ToDouble(barcode.PackingLength) / 0.9144);

                ////zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
                //WithOut Layout Option
                //zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,480^FDJalur^FS^FO175,480^FD{barcode.DocumentNo}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
                ////zplString += $"^XA^MMT^PW382^LL0635^LS0^CFB,18,10^FO20,250^FDAnyaman^FS^FO150,250^FD{barcode.MaterialName}^FS^FO20,275^FDKonstruksi^FS^FO150,275^FD{barcode.MaterialConstructionName}^FS^FO250,275^FD{barcode.YarnMaterialName}^FS^FO20,300^FDPanjang^FS^FO150,300^FD{barcode.PackingLength}^FS^FO250,300^FD{barcode.UOMSKU}^FS^FO20,325^FDMotif/Warna^FS^FO150,325^FD{barcode.Color}^FS^FO20,550^FD{DateTime.Now}^FS^FT131,169^BQN,2,5^FH\\^FDLA,{barcode.PackingCode}^FS^FT122,182^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                // With Layout Option
                if (RB_potrait.IsChecked == true)
                    //potrait
                    zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,480^FDJalur^FS^FO175,480^FD{barcode.DocumentNo}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
                else
                    //landscape
                    //zplString += $"^XA^MMT^PW520^LL0635^LS0^CFA,0,11^FO220,135^FDAnyaman^FS^FO355,135^FD{barcode.MaterialName}^FS^FO220,170^FDKonstruksi^FS^FO355,170^FD{barcode.MaterialConstructionName}^FS^FO430,170^FD{barcode.YarnMaterialName}^FS^FO220,205^FDPanjang^FS^FO355,205^FD{barcode.PackingLength}^FS^FO430,205^FD{barcode.UOMSKU}^FS^FO220,240^FDMotif/Warna^FS^FO365,240^FD{barcode.Color}^FS^FO220,275^FDJalur^FS^FO355,275^FD{barcode.DocumentNo}/{convYard}^FS^FO45,125^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT45,335^A0N,16,21^FB125,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                    zplString += $"^XA^MMT^PW600^LL0635^LS0^CF0,0,26^FO20,20^FDMaterial^FS^FO35,50^FD{barcode.MaterialName}^FS^FO20,90^FDAnyaman^FS^FO35,120^FD{barcode.MaterialConstructionName}^FS^FO235,120^FD{barcode.YarnMaterialName}^FS^FO210,155^FDLebar^FS^FO355,155^FD{barcode.MaterialWidth}^FS^FO210,195^FDPanjang^FS^FO355,195^FD{barcode.PackingLength}^FS^FO435,195^FD{barcode.UOMSKU}^FS^FO355,235^FD{convYard}^FS^FO435,235^FDYARD^FS^FO210,275^FDMotif/Warna^FS^FO355,275^FD{barcode.Color}^FS^FO25,140^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FO125,325^A0N,40,40^FB400,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

            }
            //var zpl2 = "CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNM,0^MTT^PON^PMN^LH0,0^JMA^PR3,3~SD30^JUS^LRN^CI27^PA0,1,1,0^XZ^XA^MMT^PW615^LL328^LS0^FWR^FO415,70^BQ,2,8^FDQA,FD200200002PCS^FS^FO380,80^AD,12,12^FD200200002PCS^FS^FO100,25^BY2^BC,75,Y,N,N^FD200200002^FS^FO306,65^AD,12,12^FDPC25 LOT 1^FS^FO280,65^AD,11,11^FD2 BALE^FS^FO0,25^AD,10,10^FD28/04/2020^FS^PQ1,0,1,Y^XZ";
            for (int i = 0; i < jml; i++)
            {
                ZebraPrinterHelper.Print(zplString);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (BarcodeListView.Items.)
        }

        private void PrintSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            //if (BarcodeListView.Items.Count > 0)
            //{
            //    var printBarcodeJob = new BarcodePrintJob();
            //    printBarcodeJob.PrintBartenderJob(_barcodes);
            //}

            var zplString = "";
            var barcodes = BarcodeGrid.SelectedItems.Cast<BarcodeInfo>().ToList();

            int jml = QtyPrint.Text != ""? Int32.Parse(QtyPrint.Text) : 1 ;
              

                foreach (var barcode in barcodes)
                {
                    var printedListJson = _cache.StringGet(barcode.OrderNo);
                    var printedList = new List<string>();

                    if (!string.IsNullOrWhiteSpace(printedListJson))
                    {
                        printedList = JsonConvert.DeserializeObject<List<string>>(printedListJson, new JsonSerializerSettings()
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        });

                        if (printedList == null)
                            printedList = new List<string>();
                    }

                    printedList.Add(barcode.PackingCode);
                    printedListJson = JsonConvert.SerializeObject(printedList);
                    _cache.StringSet(barcode.OrderNo, printedListJson);
                    double convYard = Math.Round(Convert.ToDouble(barcode.PackingLength) / 0.9144);
                    //WithOut Layout Option
                    //zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                    //With Layout Option
                    if (RB_potrait.IsChecked == true)
                        //potrait
                        zplString += $"^XA^MMT^PW382^LL0635^LS0^CFA,0,11^FO30,340^FDAnyaman^FS^FO175,340^FD{barcode.MaterialName}^FS^FO30,375^FDKonstruksi^FS^FO175,375^FD{barcode.MaterialConstructionName}^FS^FO260,375^FD{barcode.YarnMaterialName}^FS^FO30,410^FDPanjang^FS^FO175,410^FD{barcode.PackingLength}^FS^FO260,410^FD{barcode.UOMSKU}^FS^FO30,445^FDMotif/Warna^FS^FO175,445^FD{barcode.Color}^FS^FO30,480^FDJalur^FS^FO175,480^FD{barcode.DocumentNo}^FS^FO30,550^FD{DateTime.Now}^FS^FT110,280^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT135,310^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
                    else
                        //landscape
                        //zplString += $"^XA^MMT^PW520^LL0635^LS0^CFA,0,11^FO185,30^FDAnyaman^FS^FO320,30^FD{barcode.MaterialName}^FS^FO185,65^FDKonstruksi^FS^FO320,65^FD{barcode.MaterialConstructionName}^FS^FO395,65^FD{barcode.YarnMaterialName}^FS^FO185,100^FDPanjang^FS^FO320,100^FD{barcode.PackingLength}^FS^FO395,100^FD{barcode.UOMSKU}^FS^FO185,135^FDMotif/Warna^FS^FO320,135^FD{barcode.Color}^FS^FO185,170^FDJalur^FS^FO320,170^FD{barcode.DocumentNo}^FS^FO10,300^FD{DateTime.Now}^FS^FO1,20^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT50,230^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                        //16/02/2023
                        //zplString += $"^XA^MMT^PW520^LL0635^LS0^CFA,0,11^FO220,135^FDAnyaman^FS^FO355,135^FD{barcode.MaterialName}^FS^FO220,170^FDKonstruksi^FS^FO355,170^FD{barcode.MaterialConstructionName}^FS^FO430,170^FD{barcode.YarnMaterialName}^FS^FO220,205^FDPanjang^FS^FO355,205^FD{barcode.PackingLength}^FS^FO430,205^FD{barcode.UOMSKU}^FS^FO220,240^FDMotif/Warna^FS^FO365,240^FD{barcode.Color}^FS^FO220,275^FDJalur^FS^FO355,275^FD{barcode.DocumentNo}/{convYard}^FS^FO45,125^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FT45,335^A0N,16,21^FB125,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                        //15/03/2023
                        //zplString += $"^XA^MMT^PW600^LL0635^LS0^CF0,0,26^FO210,115^FDAnyaman^FS^FO355,115^FD{barcode.MaterialName}^FS^FO210,160^FDKonstruksi^FS^FO250,185^FD{barcode.MaterialConstructionName}^FS^FO420,185^FD{barcode.YarnMaterialName}^FS^FO210,225^FDPanjang^FS^FO355,225^FD{barcode.PackingLength}^FS^FO430,225^FD{barcode.UOMSKU}^FS^FO210,275^FDMotif/Warna^FS^FO355,275^FD{barcode.Color}^FS^FO45,125^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FO0,325^A0N,25,25^FB250,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                        //zplString += $"^XA^MMT^PW600^LL0635^LS0^CF0,0,26^FO210,115^FDAnyaman^FS^FO355,115^FD{barcode.MaterialName}^FS^FO210,160^FDKonstruksi^FS^FO250,185^FD{barcode.MaterialConstructionName}^FS^FO420,185^FD{barcode.YarnMaterialName}^FS^FO210,225^FDPanjang^FS^FO355,225^FD{barcode.PackingLength}^FS^FO430,225^FD{barcode.UOMSKU}^FS^FO210,275^FDMotif/Warna^FS^FO355,275^FD{barcode.Color}^FS^FO210,325^FDMotif/Warna^FS^FO355,325^FD{barcode.MaterialWidth}^FS^FO45,125^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FO0,325^A0N,25,25^FB250,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                    zplString += $"^XA^MMT^PW600^LL0635^LS0^CF0,0,26^FO20,20^FDMaterial^FS^FO35,50^FD{barcode.MaterialName}^FS^FO20,90^FDAnyaman^FS^FO35,120^FD{barcode.MaterialConstructionName}^FS^FO235,120^FD{barcode.YarnMaterialName}^FS^FO210,155^FDLebar^FS^FO355,155^FD{barcode.MaterialWidth}^FS^FO210,195^FDPanjang^FS^FO355,195^FD{barcode.PackingLength}^FS^FO435,195^FD{barcode.UOMSKU}^FS^FO355,235^FD{convYard}^FS^FO435,235^FDYARD^FS^FO210,275^FDMotif/Warna^FS^FO355,275^FD{barcode.Color}^FS^FO25,140^BQN,2,7^FH\\^FDLA,{barcode.PackingCode}^FS^FO125,325^A0N,40,40^FB400,2,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";

                    //zplString += $"^XA^MMT^PW382^LL0635^LS0^CFB,18,10^FO20,250^FDAnyaman^FS^FO150,250^FD{barcode.MaterialName}^FS^FO20,275^FDKonstruksi^FS^FO150,275^FD{barcode.MaterialConstructionName}^FS^FO250,275^FD{barcode.YarnMaterialName}^FS^FO20,300^FDPanjang^FS^FO150,300^FD{barcode.PackingLength}^FS^FO250,300^FD{barcode.UOMSKU}^FS^FO20,325^FDMotif/Warna^FS^FO150,325^FD{barcode.Color}^FS^FO20,550^FD{DateTime.Now}^FS^FT131,169^BQN,2,5^FH\\^FDLA,{barcode.PackingCode}^FS^FT122,182^A0N,16,21^FB125,1,0,C^FH\\^FD{barcode.PackingCode}^FS^PQ1,0,1,Y^XZ";
                }

            //var zpl2 = "CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNM,0^MTT^PON^PMN^LH0,0^JMA^PR3,3~SD30^JUS^LRN^CI27^PA0,1,1,0^XZ^XA^MMT^PW615^LL328^LS0^FWR^FO415,70^BQ,2,8^FDQA,FD200200002PCS^FS^FO380,80^AD,12,12^FD200200002PCS^FS^FO100,25^BY2^BC,75,Y,N,N^FD200200002^FS^FO306,65^AD,12,12^FDPC25 LOT 1^FS^FO280,65^AD,11,11^FD2 BALE^FS^FO0,25^AD,10,10^FD28/04/2020^FS^PQ1,0,1,Y^XZ";
            for (int i = 0; i < jml; i++)
            {

                ZebraPrinterHelper.Print(zplString);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = BarcodeList.ToArray();
            foreach (var item in selectedItems)
            {
                BarcodeList.Remove(item);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            BarcodeList.Clear();
        }

        private void BarcodeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BarcodeList.Count > 0)
            {
                DeleteButton.IsEnabled = true;
            }
        }

        //private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        //}

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToDouble(e.Text);
            }
            catch
            {
                // Show some kind of error message if you want

                // Set handled to true
                e.Handled = true;
            }
        }

        
    }
}
