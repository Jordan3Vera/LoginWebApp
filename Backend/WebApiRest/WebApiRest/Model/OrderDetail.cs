using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRest.Model
{
    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [Required]
        public int CustomerOrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required, Range(10,2)]
        public decimal Amount { get; set; }

        public CustomerOrder CustomerOrder { get; set; }
        public Product Product { get; set; }
    }
}
