using System.Collections.Generic;

namespace Core.Entity
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CompanyId { get; set; }

        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public string PhotoUri { get; set; }

        public string Description { get; set; }

        public bool IsInStock { get; set; }

        public string HexColor { get; set; }

        #region NavigationProperties

        public ICollection<ProductServing> ProductServings { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ProductType ProductType { get; set; }

        public Company Company { get; set; }

        #endregion
    }
}
