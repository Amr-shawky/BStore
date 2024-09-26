using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface IPublisherRepository
    {
        public void Add(Publisher publisher);
        public void Update(Publisher publisher);
        public void Delete(int ID);
        public List<Publisher> GetAll();
        public Publisher GetByID(int ID);
        public void Save();
    }
}
