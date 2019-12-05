using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.ViewModels
{
    public class YarnProductViewModel
    {
        public YarnProductViewModel(string lotNo, decimal quantity, string uomUnit, string yarnType)
        {
            LotNo = lotNo;
            ProductType = "YARN";
            Quantity = quantity;
            UOMUnit = uomUnit;
            YarnType1 = yarnType;
        }

        public string LotNo { get; private set; }
        public string ProductType { get; private set; }
        public decimal Quantity { get; private set; }
        public string UOMUnit { get; private set; }
        public string YarnType1 { get; private set; }
    }
}
