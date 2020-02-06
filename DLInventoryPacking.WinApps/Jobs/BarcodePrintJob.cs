using DLInventoryPacking.WinApps.Services.ResponseModel;
using Newtonsoft.Json;
using QRCoder;
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DLInventoryPacking.WinApps.Jobs
{
    public class BarcodePrintJob
    {
        public void PrintBarcode(List<BarcodeInfo> barcodes)
        {

            var index = 0;
            var grids = new List<Grid>();
            foreach (var barcode in barcodes)
            {
                var qrCodeBitmap = GetQRCodeBitmap(barcode);
                var imageSource = GetImage(qrCodeBitmap);

                var grid = new Grid
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
                var imageBorder = new Border() { BorderThickness = new Thickness(1, 1, 0, 1), BorderBrush = new SolidColorBrush(Colors.Black) };
                var textBorder = new Border() { BorderThickness = new Thickness(1, 1, 1, 1), BorderBrush = new SolidColorBrush(Colors.Black) };
                Grid.SetColumn(imageBorder, 0);
                Grid.SetColumn(textBorder, 1);

                var textStackPanel = new StackPanel();
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"Iki Jeneng Barange", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"\n", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"Iki nek ono pakan e", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textBorder.Child = textStackPanel;

                var horizontalStack = new StackPanel()
                {
                    Orientation = System.Windows.Controls.Orientation.Horizontal
                };
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"Upin", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"&", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"Ipin", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(horizontalStack);

                var stackPanel = new StackPanel()
                {
                    Orientation = System.Windows.Controls.Orientation.Vertical
                };
                imageSource.Stretch = Stretch.Fill;
                stackPanel.Children.Add(imageSource);
                stackPanel.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.PackingCode}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });

                imageSource.HorizontalAlignment = HorizontalAlignment.Center;
                imageSource.VerticalAlignment = VerticalAlignment.Center;
                imageBorder.Child = stackPanel;
                grid.Children.Add(imageBorder);
                grid.Children.Add(textBorder);

                grids.Add(grid);

                index++;
            }

            var stackPanelToPrint = new StackPanel();
            var documents = new List<FlowDocument>();
            foreach (var grid in grids)
            {
                var document = new FlowDocument();
                var uiContainer = new BlockUIContainer(grid);
                document.Blocks.Add(uiContainer);
                documents.Add(document);
            }

            var documentIndex = 1;
            foreach (var document in documents)
            {
                var printDialog = new PrintDialog();
                document.PageHeight = printDialog.PrintableAreaHeight;
                document.PageWidth = printDialog.PrintableAreaWidth;
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, $"document {documentIndex}");
            }
        }

        private FlowDocument CreateDocument(List<BarcodeInfo> barcodes)
        {
            var document = new FlowDocument();

            var table = new Table();

            for (var i = 0; i < 1; i++)
            {
                table.Columns.Add(new TableColumn());
            }

            var rowGroup = new TableRowGroup();
            table.RowGroups.Add(rowGroup);
            table.RowGroups[0].Rows.Add(new TableRow());

            var currentRow = table.RowGroups[0].Rows[0];

            foreach (var barcode in barcodes)
            {
                var bitmap = GetQRCodeBitmap(barcode);

                var image = GetImage(bitmap);

                var block = new BlockUIContainer(image);
                currentRow.Cells.Add(new TableCell(block));
            }

            document.Blocks.Add(table);
            return document;
        }

        private Bitmap GetQRCodeBitmap(BarcodeInfo barcode)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(JsonConvert.SerializeObject(barcode), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }

        private System.Windows.Controls.Image GetImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return new System.Windows.Controls.Image()
                {
                    Source = bitmapImage
                };
            };


        }

        public void PrintBartenderJob(List<BarcodeInfo> barcodes)
        {
            foreach (var barcode in barcodes)
            {
                using (var btEngine = new Engine())
                {
                    btEngine.Start();
                    var btFormat = btEngine.Documents.Open(@"C:\Users\LeslieAula\Documents\BarTender\BarTender Documents\Document1.btw");
                    btFormat.SubStrings["QRCodePayload"].Value = JsonConvert.SerializeObject(barcode);
                    btFormat.SubStrings["PackingInformation"].Value = barcode.ProductSKUName + "\n" + barcode.PackingType;
                    btFormat.SubStrings["ProductSKUQuantity"].Value = barcode.Quantity.ToString() + " " + barcode.UOMUnitSKU;
                    btFormat.SubStrings["CreatedDate"].Value = DateTime.Now.ToString();
                    //btFormat.
                    var result = btFormat.Print();

                    btEngine.Stop();
                }
            }

            //using (var btTaskManager = new TaskManager())
            //{
            //    btTaskManager.Start(1);

            //    var groupTask = new GroupTask();

            //    foreach (var barcode in barcodes)
            //    {
            //        //var btFormat = new LabelFormat(@"C:\Users\LeslieAula\Documents\BarTender\BarTender Documents\Template2DL.btw");
            //        //btFormat.SubStrings["QRCodePayload"].Value = JsonConvert.SerializeObject(barcode);
            //        //btFormat.SubStrings["PackingInformation"].Value = barcode.ProductSKUName + "\n" + barcode.PackingType;
            //        //btFormat.SubStrings["ProductSKUQuantity"].Value = barcode.Quantity.ToString() + " " + barcode.UOMUnitSKU;
            //        //btFormat.SubStrings["CreatedDate"].Value = DateTime.Now.ToString();

            //        //var printTask = new PrintLabelFormatTask(btFormat);
            //        var task = new CustomTask(@"C:\Users\LeslieAula\Documents\BarTender\BarTender Documents\Template2DL.btw", barcode);
            //        groupTask.Add(task);
            //    }

            //    btTaskManager.TaskQueue.QueueTask(groupTask);

            //    btTaskManager.Stop(5000, true);
            //}
        }
    }

    public class CustomTask : Task
    {
        private LabelFormat format;
        private BarcodeInfo _barcode;

        // Initialize the LabelFormat object. 
        public CustomTask(string labelFormatFileName, BarcodeInfo barcode)
        {
            format = new LabelFormat(labelFormatFileName);
            _barcode = barcode;
        }

        // Override this method to perform custom Task logic. 
        // This sample method writes values to the named 
        // substrings on the label and then prints the label. 
        protected override bool OnRun()
        {
            LabelFormatDocument formatDoc = null;
            try
            {
                // Open a LabelFormatDocument by using the LabelFormat that was passed in 
                formatDoc = Engine.Documents.Open(format, out messages);

                // Set some substrings on the label 
                formatDoc.SubStrings["QRCodePayload"].Value = JsonConvert.SerializeObject(_barcode);
                formatDoc.SubStrings["PackingInformation"].Value = _barcode.ProductSKUName + "\n" + _barcode.PackingType;
                formatDoc.SubStrings["ProductSKUQuantity"].Value = _barcode.Quantity.ToString() + " " + _barcode.UOMUnitSKU;
                formatDoc.SubStrings["CreatedDate"].Value = DateTime.Now.ToString();
                formatDoc.PrintSetup.UseDatabase = false;

                // Print the label 
                Messages printMessages = null;
                formatDoc.Print("", 1000, out printMessages);
                foreach (Message message in printMessages)
                {
                    messages.Add(message);
                }

                // Close the LabelFormatDocument to free up resources 
                // that were used in the TaskEngine 
                formatDoc.Close(SaveOptions.DoNotSaveChanges);

                // Assign this to the member LabelFormat variable so that 
                // it can be accessed after the Task finishes 
                format = formatDoc;
            }
            catch
            {
                try
                {
                    // Try to close the format if it is still open 
                    formatDoc.Close(SaveOptions.DoNotSaveChanges);
                }
                catch (Exception)
                {

                }
                throw;
            }
            return base.OnRun();
        }

        // Override this method to verify that this Task's properties 
        // are correct when it is added to the TaskQueue. Typically, 
        // you would throw an exception that you would catch during a 
        // TaskManager.TaskQueue.QueueTask call. 
        protected override void OnValidate()
        {
        }

        // Allow access to the LabelFormat that is used in this Task. 
        public LabelFormat LabelFormat
        {
            get
            {
                return format;
            }
        }
    }
}
