using BStore.Models.Context;

namespace BStore.Repository
{
    public class SearchUserName : ISearchUserName
    {
        BStore_Context context;
        public SearchUserName(BStore_Context bStore_Context) {
            context = bStore_Context;
        
        }
        public bool IsUserNameUnique(string userName)
        {
            var user = context.Users.FirstOrDefault(x => x.UserName == userName);
            return (user == null);
        }
    }
}
