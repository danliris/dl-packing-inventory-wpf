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
        public string MaterialName { get; set; }
        public string MaterialConstructionName { get; set; }
        public string YarnMaterialName { get; set; }
        public string PackingLength { get; set; }
        public string PackingType { get; set; }
        public string Color { get; set; }
        public string OrderNo { get; set; }
        public string UOMSKU { get; set; }
    }

    public class NewBarcodeInfo
    {
        public NewBarcodeInfo()
        {
            productPackingCodes = new List<string>();
        }

        public List<string> productPackingCodes { get; set; }
        public NameOnly material { get; set; }
        public NameOnly materialConstruction { get; set; }
        public NameOnly yarnMaterial { get; set; }
        public double productPackingLength { get; set; }
        public string productPackingType { get; set; }
        public string color { get; set; }
        public string uomUnit { get; set; }
        public ProductionOrder productionOrder { get; set; }

    }

    public class NameOnly
    {
        public string name { get; set; }
    }

    public class ProductionOrder
    {
        public string no { get; set; }
    }
}
