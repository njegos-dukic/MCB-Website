using System.Collections.Generic;

namespace Core.DTO
{
    public class OutputMenuItemDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Ingredients { get; set; }

        public List<OutputMenuItemServingDTO> Servings { get; set; }
    }
}
