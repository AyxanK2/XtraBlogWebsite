using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XtraBlogWebsite.ViewsModel;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DashboardController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Profile(UserVM model)
        {
            if(!ModelState.IsValid) return View(model);
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if(model.NewPassword != null)
            {
                bool CheckPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                if(!CheckPassword) 
                {
                    ModelState.AddModelError("OldPassword", "Please,enter your password");
                    return View(model);
                }
                if(model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("OldPassword", "Please,confirm your password");
                    return View(model);
                }
                await _userManager.ChangePasswordAsync(user,model.OldPassword,model.NewPassword);
            }
            user.Email = model.Email;
            user.UserName = model.UserName;
            await _userManager.UpdateAsync(user);
            TempData["Message"] = "Your password has change successfully";
            return RedirectToAction(nameof(Profile));
        }
    }
}
