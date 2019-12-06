using DLInventoryPacking.WinApps.Enums;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using DLInventoryPacking.WinApps.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services
{
    public static class PackingInventoryService
    {
        public static async Task<BarcodeInfo> PostProduct(ProductViewModel viewModel)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserCredentials.Token);

            var requestContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(APIEndpoint.PackingInventoryEndpoint + "product-packing-skus/retreive-qrcode-info", requestContent);

            var barcodeResult = new BaseResponseBarcodeInfo<BarcodeInfo>();
            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                var jsonSerializerSetting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };

                barcodeResult = JsonConvert.DeserializeObject<BaseResponseBarcodeInfo<BarcodeInfo>>(responseContentString, jsonSerializerSetting);
            }

            httpClient.Dispose();
            return barcodeResult.BarcodeInfo;
        }
    }
}
