using Microsoft.EntityFrameworkCore;

namespace BStore_MVC.Models.Context
{
    public class Bstore_System:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.;Database =BStore ;integrated security = true ;Encrypt=False;Trust Server Certificate=True ");
        }
        public DbSet<Person> persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Author> authors { get; set; }
        public DbSet <User> users { get; set; }

    }
}
