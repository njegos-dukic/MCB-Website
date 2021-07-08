using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper.Product
{
    public class ProductPutWrapper
    {
        /// <summary>
        /// Unique identifer for the product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ingredients of the product
        /// </summary>
        public List<string> Ingredients { get; set; }

        /// <summary>
        /// Is the product available in stock?
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// Can the product be purchased online?
        /// </summary>
        public bool CanBePurchasedOnline { get; set; }

        /// <summary>
        /// Hexadecimal value for a color
        /// </summary>
        public string HexColor { get; set; }

        /// <summary>
        /// Unique identifier for the type of the product
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// List of the possible servings of the product with price
        /// </summary>
        public List<ProductServingWrapper> ProductServings { get; set; }
    }
}
