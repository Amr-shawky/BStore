using BStore.Models;

namespace BStore.Repository
{
    public interface ICountryRepository
    {
        public void Add(Country country);
        public void Update(Country country);
        public void Delete(int ID);
        public List<Country> GetAll();
        public Country GetByID(int ID);
        public void Save();
    }
}
