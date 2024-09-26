using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? TotalAmount { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public bool DelivaryStatus { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderBook> OrderBook{ get; set; }
        public Shipping Shipping { get; set; }
    }
}
