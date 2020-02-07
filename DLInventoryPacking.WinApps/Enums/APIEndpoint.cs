using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Enums
{
    public static class APIEndpoint
    {
        public const string AuthEndpoint = "http://dl-auth-api-dev.azurewebsites.net/v1/";
        public const string PackingInventoryEndpoint = "http://com-danliris-service-packing-inventory-dev.azurewebsites.net/v1/";
        public static HttpClient HttpClient = new HttpClient();
    }
}
