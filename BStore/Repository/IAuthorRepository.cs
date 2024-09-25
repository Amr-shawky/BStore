using BStore.Models;

namespace BStore.Repository
{
    public interface IAuthorRepository
    {
            public void Add(Author author );
            public void Update(Author author);
            public void Delete(int ID);
            public List<Author> GetAll();
            public Author GetByID(int ID);
            public void Save();

        }
    }

