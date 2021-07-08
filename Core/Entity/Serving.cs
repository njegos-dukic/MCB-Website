using System.Collections.Generic;

namespace Core.Entity
{
    public class Serving
    {
        public int ServingId { get; set; }

        public string Name { get; set; }

        #region NavigationProperties
        public ICollection<ProductServing> ProductServings { get; set; }
        #endregion
    }
}
