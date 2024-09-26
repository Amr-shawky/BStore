using BKStore_MVC.Models;

namespace BKStore_MVC.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        public void Add(Customer customer);
        public void Update(Customer customer);
        public void Delete(int ID);
        public List<Customer> GetAll();
        public Customer GetByID(int ID);
        public void Save();
    }
}
