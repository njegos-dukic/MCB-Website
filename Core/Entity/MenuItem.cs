namespace Core.Entity
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        public int MenuId { get; set; }

        public int ProductServingId { get; set; }

        #region NavigationProperties
        public Menu Menu { get; set; }

        public ProductServing ProductServing { get; set; }
        #endregion
    }
}
