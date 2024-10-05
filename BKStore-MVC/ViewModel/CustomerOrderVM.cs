using BKStore_MVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.ViewModel
{
    public class CustomerOrderVM
    {
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        [Display(Name ="Full Name")]
        public string Name { get; set; }
        [StringLength(400, ErrorMessage = " Address cannot exceed 400 characters.")]
        public string Address { get; set; }
        [StringLength(15, ErrorMessage = " Phone cannot exceed 15 characters.")]
        [Display(Name = "Mobile")]
        public string Phone { get; set; }
        public string? UserID { get; set; }
        [Required]
        public int? GovernorateID { get; set; }
        public int? BookID { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public decimal? TotalAmount { get; set; }
        
        public List<BookCartItem>? BookItems { get; set; }
        //public int? BookPrice { get; set; }


    }
}
