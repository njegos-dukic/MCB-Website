using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class OutputOrderDTO : BaseOrderDTO
    {
        public int OrderId { get; set; }

        public List<OutputProductOrderDTO> ProductOrders { get; set; }
    }
}
