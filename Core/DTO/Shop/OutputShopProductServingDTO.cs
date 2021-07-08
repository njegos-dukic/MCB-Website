namespace Core.DTO
{
    public class OutputShopProductServingDTO : ShopProductServingDTO
    {
        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ServingName { get; set; }

        public int PackageAmount { get; set; }

        public int IncrementAmount { get; set; }

        public string GetConcatenatedName()
            => ProductName + " " + ServingName;
    }
}
