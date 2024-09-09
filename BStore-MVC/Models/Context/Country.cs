using System.ComponentModel.DataAnnotations;

namespace BStore_MVC.Models.Context
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        [Required]
        public string CountryName { get; set; }
    }
}
