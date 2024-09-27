using BKStore_MVC.ViewModel;
using BKStore_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BKStore_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View("Register");
        }
        public async Task<IActionResult> SaveRegister(RegisterBS viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = viewModel.UserName;
                applicationUser.PasswordHash = viewModel.Password;
                applicationUser.Email = viewModel.Email;
                IdentityResult result = await userManager.CreateAsync(applicationUser, viewModel.Password);
                if (result.Succeeded)
                {
                    //     await userManager.AddToRoleAsync(applicationUser, "Admin");
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", viewModel);
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        public async Task<IActionResult> SaveLogin(LoginBS loginBS)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser =
                    await userManager.FindByNameAsync(loginBS.UserName);

                if (appuser != null)
                {
                    bool found =
                        await userManager.CheckPasswordAsync(appuser, loginBS.Password);
                    if (found == true)
                    {
                        await signInManager.SignInAsync(appuser, loginBS.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Username OR Password Wrong");
            }
            return View("Login", loginBS);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View(nameof(Register));
        }
        public IActionResult AddAdminAccount()
        {
            return View("Register");
        }
        public async Task<IActionResult> SaveAdmin(RegisterBS viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = viewModel.UserName;
                applicationUser.PasswordHash = viewModel.Password;
                applicationUser.Email = viewModel.Email;
                IdentityResult result = await userManager.CreateAsync(applicationUser, viewModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "Admin");
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", viewModel);
        }

    }
}