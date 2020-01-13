using DLInventoryPacking.WinApps.Services.ResponseModel;
using Newtonsoft.Json;
using QRCoder;
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
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.SKUId}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.Quantity}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.Code}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textBorder.Child = textStackPanel;

                var horizontalStack = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.UOMUnit}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"\n", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                horizontalStack.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.PackingType}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                textStackPanel.Children.Add(horizontalStack);

                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };
                imageSource.Stretch = Stretch.Fill;
                stackPanel.Children.Add(imageSource);
                stackPanel.Children.Add(new Label() { FontSize = 10, Content = $"{barcode.Code}", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });

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
    }
}
