using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper.Menu
{
    public class MenuPutWrapper
    {
        /// <summary>
        /// Unique identifier for the menu
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Name of the menu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// menu's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Items on the menu
        /// </summary>
        public List<MenuItemPutWrapper> MenuItems { get; set; }
    }
}
