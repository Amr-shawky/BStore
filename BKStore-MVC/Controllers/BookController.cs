using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
