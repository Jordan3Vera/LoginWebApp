using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApiRest.Model.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
