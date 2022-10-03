using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiRest.Model.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<CustomerOrderDTO> CustomerOrder { get; set; }
    }
}
