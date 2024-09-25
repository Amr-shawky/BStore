using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class OrderRepository:IOrderRepository
    {
        BStore_Context context;
        public OrderRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Order order)
        {
            context.Add(order);
        }

        public void Delete(int ID)
        {
            Order order = GetByID(ID);
            context.Remove(order);
        }

        public List<Order> GetAll()
        {
            return context.Order.ToList();
        }

        public Order GetByID(int ID)
        {
            return context.Order.FirstOrDefault(c => c.OrderId== ID) ?? new Order();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Order order)
        {
            context.Update(order);
        }
    }
}
