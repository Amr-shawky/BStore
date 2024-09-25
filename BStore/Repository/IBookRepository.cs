using BStore.Models;

namespace BStore.Repository
{
    public interface IBookRepository
    {
        public void Add(Book book);
        public void Update(Book book);
        public void Delete(int ID);
        public List<Book> GetAll();
        public Book GetByID(int ID);
        public void Save();
    }
}
