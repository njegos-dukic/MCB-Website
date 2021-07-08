using System.Collections.Generic;

namespace Core.DTO.Menu
{
    public class MenuDTO
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<MenuItemDTO> MenuItems { get; set; }
    }
}
