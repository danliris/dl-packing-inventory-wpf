using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.ViewModels
{
    public class FabricProductViewModel
    {
        public FabricProductViewModel(string composition, string construction, string design, string grade, string packType, decimal quantity, string uomUnit, string width)
        {
            Composition = composition;
            Construction = construction;
            Design = design;
            Grade = grade;
            PackType = packType;
            ProductType = "FABRIC";
            Quantity = quantity;
            UOMUnit = uomUnit;
            Width = width;
        }

        public string Composition { get; private set; }
        public string Construction { get; private set; }
        public string Design { get; private set; }
        public string Grade { get; private set; }
        public string PackType { get; private set; }
        public string ProductType { get; private set; }
        public decimal Quantity { get; private set; }
        public string UOMUnit { get; private set; }
        public string Width { get; private set; }
    }
}
