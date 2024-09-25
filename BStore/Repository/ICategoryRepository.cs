using BStore.Models;

namespace BStore.Repository
{
    public interface ICategoryRepository
    {
        public void Add(Category category);
        public void Update(Category category);
        public void Delete(int ID);
        public List<Category> GetAll();
        public Category GetByID(int ID);
        public void Save();
    }
}
