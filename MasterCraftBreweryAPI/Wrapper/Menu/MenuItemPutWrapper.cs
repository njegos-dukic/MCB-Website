namespace MasterCraftBreweryAPI.Wrapper.Menu
{
    public class MenuItemPutWrapper
    {
        /// <summary>
        /// Unique identifier for the menu item
        /// </summary>
        public int MenuItemId { get; set; }

        /// <summary>
        /// Unique identifier for the menu that this item belongs to
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Product serving on the menu
        /// </summary>
        public int ProductServingId { get; set; }
    }
}
