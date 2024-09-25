using BStore.Models;
using Microsoft.AspNetCore.Identity;

namespace BStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public Reviews Reviews { get; set; }
        public Customer Customer { get; set; }
    }
}
