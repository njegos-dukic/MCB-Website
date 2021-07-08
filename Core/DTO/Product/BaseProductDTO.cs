using System.Collections.Generic;

namespace Core.DTO
{
    public class BaseProductDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Ingredients { get; set; }

        public bool IsInStock { get; set; }

        public bool CanBePurchasedOnline { get; set; }

        public string HexColor { get; set; }
    }
}
