using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BStore.Models
{
    public class OrderBook
    {
        [Key]
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public int TSubPrice { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
