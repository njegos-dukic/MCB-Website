namespace Core.Entity
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        #region NavigationProperties

        public Product Product { get; set; }

        #endregion
    }
}
