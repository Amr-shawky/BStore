using BKStore_MVC.Models;
using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class BookRepository : IBookRepository
    {
        BKstore_System context;
        public BookRepository(BKstore_System _context)
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
            return context.Book.FirstOrDefault(c => c.BookID== ID);
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
