using BKStore_MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace BKStore_MVC.Models
{
    public class ApplicationUser:IdentityUser
    {
        public Reviews Reviews { get; set; }
        public Customer Customer { get; set; }
    }
}
