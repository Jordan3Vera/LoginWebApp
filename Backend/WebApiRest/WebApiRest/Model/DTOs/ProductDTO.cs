using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApiRest.Model.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public List<OrderDetailDTO> OrderDetail { get; set; }
    }
}
