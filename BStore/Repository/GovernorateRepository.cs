using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class GovernorateRepository:IGovernorateRepository
    {
        BStore_Context context;
        public GovernorateRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Governorate governorate)
        {
            context.Add(governorate);
        }

        public void Delete(int ID)
        {
            Governorate governorate = GetByID(ID);
            context.Remove(governorate);
        }

        public List<Governorate> GetAll()
        {
            return context.governorate.ToList();
        }

        public Governorate GetByID(int ID)
        {
            return context.governorate.FirstOrDefault(c => c.Id== ID) ?? new Governorate();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Governorate governorate)
        {
            context.Update(governorate);
        }
    }
}
