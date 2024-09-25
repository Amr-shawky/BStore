using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        BStore_Context context;
        public CategoryRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Category category)
        {
            context.Add(category);
        }

        public void Delete(int ID)
        {
            Category category = GetByID(ID);
            context.Remove(category);
        }

        public List<Category> GetAll()
        {
            return context.Category.ToList();
        }

        public Category GetByID(int ID)
        {
            return context.Category.FirstOrDefault(c => c.CategoryID== ID) ?? new Category();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Category category)
        {
            context.Update(category);
        }
    }
}
