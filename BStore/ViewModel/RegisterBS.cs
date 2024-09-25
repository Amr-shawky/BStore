using BStore.Models;
using System.ComponentModel.DataAnnotations;

namespace BStore.ViewModel
{
    public class RegisterBS
    {
        [Unique]
        public string UserName { get; set; }
        
        public string Email { get; set; }



        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
