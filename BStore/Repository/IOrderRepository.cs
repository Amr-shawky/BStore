using BStore.Models;

namespace BStore.Repository
{
    public interface IOrderRepository
    {
        public void Add(Order order);
        public void Update(Order order);
        public void Delete(int ID);
        public List<Order> GetAll();
        public Order GetByID(int ID);
        public void Save();
    }
}
