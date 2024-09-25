using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class CountryRepository:ICountryRepository
    {
        BStore_Context context;
        public CountryRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Country country)
        {
            context.Add(country);
        }

        public void Delete(int ID)
        {
            Country country = GetByID(ID);
            context.Remove(country);
        }

        public List<Country> GetAll()
        {
            return context.Country.ToList();
        }

        public Country GetByID(int ID)
        {
            return context.Country.FirstOrDefault(c => c.ID== ID) ?? new Country();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Country country)
        {
            context.Update(country);
        }
    }
}
