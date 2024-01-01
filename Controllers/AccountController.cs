using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XtraBlogWebsite.ViewsModel;

namespace XtraBlogWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if(!ModelState.IsValid) return View(model);  
            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null)
            {
                ModelState.AddModelError("", "Wrong credentials");
                return View();
            }

            var signinresult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!signinresult.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials");
                return View();
            }

            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
        }

        public async Task<IActionResult> CreateUser()
        {
            IdentityUser user = new()
            {
                Email = "admin@admin.com",
                UserName = "admin",
            };

            var result = await _userManager.CreateAsync(user,"12345678");/// sifre a12345678 admin panel - admin@admin.com
            if (!result.Succeeded) return Json(result.Errors);
            return Content("Ok");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
