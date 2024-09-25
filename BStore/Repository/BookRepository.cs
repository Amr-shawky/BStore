using BStore.Models;
using BStore.Models.Context;

namespace BStore.Repository
{
    public class BookRepository : IBookRepository
    {
        BStore_Context context;
        public BookRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Book book)
        {
            context.Add(book);
        }

        public void Delete(int ID)
        {
            Book book = GetByID(ID);
            context.Remove(book);
        }

        public List<Book> GetAll()
        {
            return context.Book.ToList();
        }

        public Book GetByID(int ID)
        {
            return context.Book.FirstOrDefault(c => c.BookID== ID) ?? new Book();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Book book)
        {
            context.Update(book);
        }
    }
}
