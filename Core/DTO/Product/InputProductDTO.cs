using Core.Util;
using System.Collections.Generic;

namespace Core.DTO
{
    public class InputProductDTO : BaseProductDTO
    {
        public int ProductTypeId { get; set; }

        public BasicFileInfo PhotoInfo { get; set; }

        public List<ProductServingDTO> ProductServings { get; set; }
    }
}
