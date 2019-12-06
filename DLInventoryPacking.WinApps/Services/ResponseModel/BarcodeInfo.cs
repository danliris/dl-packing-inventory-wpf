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
        public string Code { get; set; }
        public int SKUId { get; set; }
        public decimal Quantity { get; set; }
        public string UOMUnit { get; set; }
        public string PackingType { get; set; }
        public int PackingId { get; set; }
    }
}
