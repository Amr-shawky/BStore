using BStore.Models.Context;
using BStore.Models;

namespace BStore.Repository
{
    public class ReviewsRepository:IReviewsRepository
    {
        BStore_Context context;
        public ReviewsRepository(BStore_Context _context)
        {
            context = _context;
        }
        public void Add(Reviews reviews)
        {
            context.Add(reviews);
        }

        public void Delete(int ID)
        {
            Reviews reviews = GetByID(ID);
            context.Remove(reviews);
        }

        public List<Reviews> GetAll()
        {
            return context.Reviews.ToList();
        }

        public Reviews GetByID(int ID)
        {
            return context.Reviews.FirstOrDefault(c => c.ReviewId == ID) ?? new Reviews();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Reviews reviews)
        {
            context.Update(reviews);
        }
    }
}
