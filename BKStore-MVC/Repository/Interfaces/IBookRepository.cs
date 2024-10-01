using System.Collections.Generic;
using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IBookRepository
    {
        public void Add(Book book);
        public void Update(Book book);
        public void Delete(int ID);
        public List<Book> GetAll();
        public List<Book> GetByName(string name);
        public Book GetByID(int ID);
        public List<Book> GetBooksByCatgyId (int id); 
        public void Save();
    }
}
