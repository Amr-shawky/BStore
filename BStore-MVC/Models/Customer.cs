using System.ComponentModel.DataAnnotations;

namespace BStore_MVC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }


    }
}
