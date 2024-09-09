namespace BStore_MVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public int TotalAmount { get; set; }

    }
}
