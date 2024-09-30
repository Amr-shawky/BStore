using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class DeliveryController : Controller
    {
        UserManager<ApplicationUser> UserManager;
        private readonly IDeliveryClientRepository deliveryClientRepository;

        public DeliveryController(UserManager<ApplicationUser> userManager,
            IDeliveryClientRepository deliveryClientRepository)
        {
            UserManager = userManager;
            this.deliveryClientRepository = deliveryClientRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddDelivery(string UserId)
        {
            //string userId = Request.Cookies["UserId"]??"";
            ApplicationUser appuser = await UserManager.FindByIdAsync(UserId);
            if (appuser != null)
            {
                DeliveryVM deliveryVM = new DeliveryVM();
                deliveryVM.Email = appuser.Email;
                deliveryVM.UserName = appuser.UserName;
                deliveryVM.UserID = UserId;
                return View("AddDelivery", deliveryVM);

            }


            return View("AddDelivery");
        }
        public async Task<IActionResult> SaveAdd(DeliveryVM deliveryVM)
        {
            ApplicationUser appuser =
                   await UserManager.FindByIdAsync(deliveryVM.UserID);
            if (appuser != null)
            {
                appuser.Email = deliveryVM.Email;
                appuser.UserName = deliveryVM.UserName;
                appuser.PhoneNumber = deliveryVM.Phone;
                appuser.LockoutEnabled = true;
                await UserManager.UpdateAsync(appuser);
                DeliveryClients deliveryClients = new DeliveryClients();
                deliveryClients.FullName = deliveryVM.FullName;
                deliveryClients.NationalID = deliveryVM.NationalID;
                deliveryClients.IsLocked = true;
                deliveryClients.UserID = appuser.Id;
                deliveryClientRepository.Add(deliveryClients);
                deliveryClientRepository.Save();
                return RedirectToAction("Index", "Home");
            }
            return View(nameof(AddDelivery), deliveryVM);
        }
    }
}
