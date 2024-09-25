using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class ShippingRepository:IShippingRepository
    {
        BStore_Context context;
        public ShippingRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Shipping shipping)
        {
            context.Add(shipping);
        }

        public void Delete(int ID)
        {
            Shipping shipping = GetByID(ID);
            context.Remove(shipping);
        }

        public List<Shipping> GetAll()
        {
            return context.Shipping.ToList();
        }

        public Shipping GetByID(int ID)
        {
            return context.Shipping.FirstOrDefault(c => c.ShippingID== ID) ?? new Shipping();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Shipping shipping)
        {
            context.Update(shipping);
        }
    }
}
