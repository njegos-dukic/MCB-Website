using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper.Product
{
    public class ProductServingWrapper : ProductServingPostWrapper
    {
        /// <summary>
        /// Unique identifier for the serving of the product
        /// </summary>
        public int ProductServingId { get; set; }

        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        public int ProductId { get; set; }
    }
}
