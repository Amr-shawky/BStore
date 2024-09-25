using BStore.Models;
using BStore.Models.Context;

namespace BStore.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        BStore_Context context;
        public AuthorRepository(BStore_Context _context)
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
            return context.Author.FirstOrDefault(c => c.AuthorId == ID) ?? new Author();
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
