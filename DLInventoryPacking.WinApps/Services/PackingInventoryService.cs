using DLInventoryPacking.WinApps.Enums;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLInventoryPacking.WinApps.Services
{
    public static class PackingInventoryService
    {
        //public static async Task<BarcodeInfo> PostProduct(ProductViewModel viewModel)
        //{
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserCredentials.Token);

        //    var requestContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

        //    var response = await httpClient.PostAsync(APIEndpoint.PackingInventoryEndpoint + "product-packing-skus/retreive-qrcode-info", requestContent);

        //    var barcodeResult = new BaseResponseBarcodeInfo<BarcodeInfo>();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContentString = await response.Content.ReadAsStringAsync();

        //        var jsonSerializerSetting = new JsonSerializerSettings()
        //        {
        //            MissingMemberHandling = MissingMemberHandling.Ignore,
        //        };

        //        barcodeResult = JsonConvert.DeserializeObject<BaseResponseBarcodeInfo<BarcodeInfo>>(responseContentString, jsonSerializerSetting);
        //    }

        //    httpClient.Dispose();
        //    return barcodeResult.BarcodeInfo;
        //}

        public static async Task<List<NewBarcodeInfo>> GetBarcodeInfoByOrderNo(string orderNo, bool isReprint, bool isStockOpname)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserCredentials.Token);

            var response = await httpClient.GetAsync(APIEndpoint.PackingInventoryEndpoint + $"dyeing-printing-product/packing?page=1&size=2000&keyword={orderNo}&isReprint={isReprint}&isStockOpname={isStockOpname}");

            // test purpose
            //var response = await httpClient.GetAsync(APIEndpoint.PackingInventoryEndpoint + $"dyeing-printing-product/packing?page=1&size=2000&keyword={orderNo}");

            var barcodeResult = new BaseResponseBarcodeInfo<List<NewBarcodeInfo>>();
            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                var jsonSerializerSetting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };

                barcodeResult = JsonConvert.DeserializeObject<BaseResponseBarcodeInfo<List<NewBarcodeInfo>>>(responseContentString, jsonSerializerSetting);
            } else
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    MessageBox.Show("Status Code: 500", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            httpClient.Dispose();
            return barcodeResult.data;
        }
    }
}
