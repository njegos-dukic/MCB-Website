using System.Collections.Generic;

namespace MasterCraftBreweryAPI.Wrapper.Menu
{
    public class MenuPostWrapper
    {
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
        public List<MenuItemPostWrapper> MenuItems { get; set; }
    }
}
