namespace Core.DTO
{
    public class OutputProductOrderDTO
    {
        public int ProductOrderId { get; set; }

        public double Price { get; set; }

        public int TotalAmount { get; set; }

        public OutputShopProductServingDTO Details { get; set; }
    }
}
