using DLInventoryPacking.WinApps.Services.ResponseModel;
using Newtonsoft.Json;
using QRCoder;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            var document = CreateDocument(barcodes);

            document.Name = "Document";

            var documentPaginatorSource = (IDocumentPaginatorSource)document;

            var printDialog = new PrintDialog();
            printDialog.PrintDocument(documentPaginatorSource.DocumentPaginator, "Printing");
        }

        private FlowDocument CreateDocument(List<BarcodeInfo> barcodes)
        {
            var document = new FlowDocument();

            var table = new Table();

            for (var i = 0; i < 4; i++)
            {
                table.Columns.Add(new TableColumn());
            }

            var rowGroup = new TableRowGroup();
            table.RowGroups.Add(rowGroup);
            table.RowGroups[0].Rows.Add(new TableRow());

            var currentRow = table.RowGroups[0].Rows[0];

            foreach (var barcode in barcodes)
            {
                var qrCodeGenerator = new QRCodeGenerator();
                var qrCodeData = qrCodeGenerator.CreateQrCode(JsonConvert.SerializeObject(barcode), QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var bitmap = qrCode.GetGraphic(20);

                var image = new System.Windows.Controls.Image()
                {
                    Source = BitmapToImageSource(bitmap)
                };
                var block = new BlockUIContainer(image);
                currentRow.Cells.Add(new TableCell(block));
            }

            document.Blocks.Add(table);
            return document;
        }

        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            using(var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
