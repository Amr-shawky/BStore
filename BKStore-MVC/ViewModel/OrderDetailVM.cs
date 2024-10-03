using BKStore_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModel
{
    public class OrderDetailVM
    {
        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }
        [Display(Name = "Address")]
        public string? CustomerAddress { get; set; }
        public string? Governorate { get; set; }
        //public string? BookName { get; set; }
        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }
        //public int? Quantity { get; set; }
        public List<BookCartItem>? bookCartItems { get; set; }

    }
}
