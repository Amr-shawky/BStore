using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BStore.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [StringLength(30, ErrorMessage = " name cannot exceed 30 characters.")]
        public string? Title { get; set; }
        [ForeignKey("Author")]
        public int? AuthorID { get; set; }
        public string? ISBN { get; set; }
        public double Price { get; set; }
        [ForeignKey("Publisher")]
        public int? PublisherID { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? StockQuantity { get; set; }
        public int? CategoryID { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderBook> orderDetails { get; set; }
        public ICollection<Reviews> books { get; set; }
    }
}
