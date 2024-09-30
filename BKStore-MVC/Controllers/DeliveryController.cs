using BKStore_MVC.Models;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class DeliveryController : Controller
    {
        UserManager<ApplicationUser> UserManager;
        public DeliveryController(UserManager<ApplicationUser> userManager) 
        {
            UserManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddDelivery()
        {
            string userId = Request.Cookies["UserId"]??"";
            ApplicationUser appuser = await UserManager.FindByIdAsync(userId);
            if (appuser != null) 
            {
                DeliveryVM deliveryVM = new DeliveryVM();
                deliveryVM.Email = appuser.Email;
                deliveryVM.UserName = appuser.UserName;
                return View("AddDelivery", deliveryVM);

            }


            return View("AddDelivery");
        }
    }
}
