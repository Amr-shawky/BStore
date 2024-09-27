using BKStore_MVC.Models.Context;
using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class PublisherRepository:IPublisherRepository
    {
        BKstore_System context;
        public PublisherRepository(BKstore_System _context)
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
            return context.Publisher.FirstOrDefault(c => c.ID== ID);
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
