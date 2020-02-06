using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services.ResponseModel
{
    public class BarcodeInfo
    {
        public string PackingCode { get; set; }
        public int SKUId { get; set; }
        public decimal Quantity { get; set; }
        public string UOMUnitSKU { get; set; }
        public string PackingType { get; set; }
        public int PackingId { get; set; }
        public string ProductSKUName { get; set; }
    }
}
