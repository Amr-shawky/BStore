using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
