using BKStore_MVC.Models;

namespace BKStore_MVC.ViewModel
{
    public class BookCategVM
    {
        public List<Book> books {  get; set; }
        public List<Category> categories { get; set; }
    }
}
