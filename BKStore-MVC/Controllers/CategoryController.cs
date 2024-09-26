using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
