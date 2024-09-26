using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
