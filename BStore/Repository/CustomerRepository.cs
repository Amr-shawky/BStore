using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        BStore_Context context;
        public CustomerRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Customer customer)
        {
            context.Add(customer);
        }

        public void Delete(int ID)
        {
            Customer customer = GetByID(ID);
            context.Remove(customer);
        }

        public List<Customer> GetAll()
        {
            return context.Customer.ToList();
        }

        public Customer GetByID(int ID)
        {
            return context.Customer.FirstOrDefault(c => c.ID== ID) ?? new Customer();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            context.Update(customer);
        }
    }
}
