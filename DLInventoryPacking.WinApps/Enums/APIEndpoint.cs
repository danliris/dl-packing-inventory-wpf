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
        //dev
        //public const string AuthEndpoint = "http://com-danliris-service-auth-dev.azurewebsites.net/v1/";
        //public const string PackingInventoryEndpoint = "http://com-danliris-service-packing-inventory-dev.azurewebsites.net/v1/";


        //uat
        public const string AuthEndpoint = "https://com-danliris-service-auth-uat.azurewebsites.net/v1/";
        public const string PackingInventoryEndpoint = "https://com-danliris-service-packing-inventory-uat.azurewebsites.net/v1/";

        //public const string AuthEndpoint = "https://com-danliris-service-auth.azurewebsites.net/v1/";
        //public const string PackingInventoryEndpoint = "https://com-danliris-service-packing-inventory.azurewebsites.net/v1/";
        //public const string PackingInventoryEndpoint = "http://localhost:5002/v1/";


        public static HttpClient HttpClient = new HttpClient();
    }
}
