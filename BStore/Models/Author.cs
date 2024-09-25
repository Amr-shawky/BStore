using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BStore.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(600, ErrorMessage = " name cannot exceed 600 characters.")]
        public string? Biography { get; set; }
        [ForeignKey(nameof(Country))]
        public int? CountryID { get; set; }
        public ICollection<Book> books { get; set; }
        public Country Country { get; set; }
    }
}
