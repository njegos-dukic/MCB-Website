using System.Collections.Generic;

namespace Core.DTO
{
    public class OutputMenuDTO
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<OutputMenuItemDTO> MenuItems { get; set; }
    }
}
