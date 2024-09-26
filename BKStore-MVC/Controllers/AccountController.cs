using BKStore_MVC.Models;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                IdentityResult result = await userManager.CreateAsync(applicationUser, viewModel.Password);
                if (result.Succeeded)
                {
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
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View(nameof(Register));
        }
        //[HttpGet]
        //public async Task<IActionResult> Register(RegisterBS register)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser applicationUser = new ApplicationUser();
        //        applicationUser.UserName = register.UserName;
        //        applicationUser.Email = register.Email;
        //        applicationUser.PasswordHash = register.Password;
        //        IdentityResult result = await userManager.CreateAsync(applicationUser, register.Password);
        //        if (result.Succeeded)
        //        {
        //            return Ok("Create");
        //        }
        //        foreach (var item in result.Errors)
        //        {
        //            ModelState.AddModelError("", item.Description);
        //        }
        //    }
        //    return BadRequest(ModelState);
        //}
        //[HttpPost(nameof(Login))]
        //public async Task<IActionResult> Login(LoginBS userfromreq)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser userfromdb =
        //             await userManager.FindByNameAsync(userfromreq.UserName);
        //        if (userfromdb != null)
        //        {
        //            bool found =
        //                 await userManager.CheckPasswordAsync(userfromdb, userfromreq.Password);
        //            if (found)
        //            {
        //                List<Claim> userclaims = new List<Claim>();

        //                //token generated ID 
        //                userclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //                userclaims.Add(new Claim(ClaimTypes.NameIdentifier, userfromdb.Id));
        //                userclaims.Add(new Claim(ClaimTypes.Name, userfromdb.UserName));
        //                var UserRole = await userManager.GetRolesAsync(userfromdb);
        //                foreach (var rolename in UserRole)
        //                {
        //                    userclaims.Add(new Claim(ClaimTypes.Role, rolename));

        //                }
        //                var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asd!@#$!#ii2@@123okpekrg%%$&(fgq35uRRTT823sdg"));
        //                SigningCredentials signingCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);
        //                //design token
        //                JwtSecurityToken mytoken = new JwtSecurityToken(
        //                    audience: "http://localhost:4200/",
        //                    issuer: "http://localhost:5088/",
        //                    expires: DateTime.Now.AddHours(1),
        //                    claims: userclaims,
        //                    signingCredentials: signingCred

        //                    );
        //                //generate token in response 
        //                return Ok(
        //                    new
        //                    {
        //                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
        //                        expiration = DateTime.Now.AddHours(1)
        //                    }
        //                    );
        //            }
        //        }
        //        ModelState.AddModelError("username", "username or password is wrong");
        //    }
        //    return BadRequest(ModelState);
        //}
    }
}







