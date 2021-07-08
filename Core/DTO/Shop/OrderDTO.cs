using System.Collections.Generic;

namespace Core.DTO
{
    public class OrderDTO : BaseOrderDTO
    {
        public List<ProductOrderDTO> ProductOrders { get; set; }
    }
}
