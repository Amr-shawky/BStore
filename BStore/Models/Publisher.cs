using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BStore.Models
{
    public class Publisher
    {
        [Key]
        public int ID { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string Name { get; set; }
        [StringLength(40, ErrorMessage = " name cannot exceed 40 characters.")]
        public string? Email { get; set; }
        [ForeignKey(nameof(Country))]
        public int? CountryID { get; set; }


        public ICollection<Book> books { get; set; }
    }
}
