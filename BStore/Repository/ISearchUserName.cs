namespace BStore.Repository
{
    public interface ISearchUserName
    {
        public bool IsUserNameUnique (string userName);
    }
}
