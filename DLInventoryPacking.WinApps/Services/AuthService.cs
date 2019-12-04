using DLInventoryPacking.WinApps.Enums;
using DLInventoryPacking.WinApps.Services.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services
{
    public static class AuthService
    {
        public static async Task<string> Authenticate(string username, string password)
        {
            var httpClient = new HttpClient();

            var authenticationBody = new { username, password };
            var requestContent = new StringContent(JsonConvert.SerializeObject(authenticationBody), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(APIEndpoint.AuthEndpoint + "authenticate", requestContent);

            var tokenResult = new BaseResponse<string>();
            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                var jsonSerializerSetting = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                tokenResult = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContentString, jsonSerializerSetting);
            }

            return tokenResult.data;
        }
    }
}
