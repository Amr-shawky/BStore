using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class PublisherRepository:IPublisherRepository
    {
        BStore_Context context;
        public PublisherRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Publisher publisher)
        {
            context.Add(publisher);
        }

        public void Delete(int ID)
        {
            Publisher publisher = GetByID(ID);
            context.Remove(publisher);
        }

        public List<Publisher> GetAll()
        {
            return context.Publisher.ToList();
        }

        public Publisher GetByID(int ID)
        {
            return context.Publisher.FirstOrDefault(c => c.ID== ID) ?? new Publisher();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Publisher publisher)
        {
            context.Update(publisher);
        }
    }
}
