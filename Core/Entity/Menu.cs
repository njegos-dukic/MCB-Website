using System.Collections.Generic;

namespace Core.Entity
{
    public class Menu
    {
        public int MenuId { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }
        #endregion
    }
}
