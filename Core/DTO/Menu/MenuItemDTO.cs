namespace Core.DTO.Menu
{
    public class MenuItemDTO
    {
        public int MenuItemId { get; set; }

        public int MenuId { get; set; }

        public int ProductServingId { get; set; }

        public ProductServingDTO ProductServing { get; set; }

        public ServingDTO Serving { get; set; }

        public MenuProductDTO Product { get; set; }
    }
}
