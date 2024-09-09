using System.ComponentModel.DataAnnotations;

namespace BStore_MVC.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
