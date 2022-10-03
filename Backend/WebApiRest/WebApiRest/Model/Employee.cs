using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRest.Model
{
    [Table("Employee")]
    public partial class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(50), DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required, MaxLength(50), DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required, MaxLength(32)]
        public string Username { get; set; }

        [Required, MaxLength(32)]
        public string Password { get; set; }
    }
}
