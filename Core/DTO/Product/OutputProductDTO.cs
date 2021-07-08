using System.Collections.Generic;

namespace Core.DTO
{
    public class OutputProductDTO : BaseProductDTO
    {
        public List<OutputProductServingDTO> ProductServings { get; set; }

        public ProductTypeDTO ProductType { get; set; }
    }
}
