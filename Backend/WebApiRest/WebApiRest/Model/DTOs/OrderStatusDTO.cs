using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApiRest.Model.DTOs
{
    public class OrderStatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CustomerOrderDTO> CustomerOrder { get; set; }
    }
}
