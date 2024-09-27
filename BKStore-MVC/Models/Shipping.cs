using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class Shipping
    {
        [Key]
        public int ShippingID { get; set; }
        [ForeignKey(nameof(Order))]
        public int? OrderID { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string? ShippingMethod { get; set; }
        public DateTime? ShippingDate { get; set; }
        public int? TrackingNumber { get; set; }
        public Order? Order { get; set; }
    }
}
