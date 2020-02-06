namespace DLInventoryPacking.WinApps.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(
            string composition,
            string construction,
            string design,
            string grade,
            string lotNo,
            string productType,
            string width,
            string wovenType,
            string yarnType1,
            string yarnType2,
            string uomUnit,
            decimal quantity,
            string packType
            )
        {
            Composition = composition;
            Construction = construction;
            Design = design;
            Grade = grade;
            LotNo = lotNo;
            ProductType = productType;
            Width = width;
            WovenType = wovenType;
            YarnType1 = yarnType1;
            YarnType2 = yarnType2;
            UOMUnit = uomUnit;
            Quantity = quantity;
            PackType = packType;
        }

        // SKU Field
        public string Composition { get; private set; }
        public string Construction { get; private set; }
        public string Design { get; private set; }
        public string Grade { get; private set; }
        public string LotNo { get; private set; }
        public string ProductType { get; private set; }
        public string Width { get; private set; }
        public string WovenType { get; private set; }
        public string YarnType1 { get; private set; }
        public string YarnType2 { get; private set; }

        // Pack Field
        public string UOMUnit { get; private set; }
        public decimal Quantity { get; private set; }
        public string PackType { get; private set; }
    }
}
