using BKStore_MVC.Models;
using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        BKstore_System context;
        public AuthorRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Author author)
        {
            context.Add(author);
        }

        public void Delete(int ID)
        {
            Author author = GetByID(ID);
            context.Remove(author);
        }

        public List<Author> GetAll()
        {
            return context.Author.ToList();
        }

        public Author GetByID(int ID)
        {
            return context.Author.FirstOrDefault(c => c.AuthorId == ID);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Author author)
        {
            context.Update(author);
        }
    }
}
