using System.ComponentModel.DataAnnotations;

namespace BStore_MVC.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int PID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
