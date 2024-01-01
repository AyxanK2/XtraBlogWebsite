using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly ApplicationContext _context;
        public SettingController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Setting? setting = _context.Settings.FirstOrDefault();
            return View(setting);
        }

        [HttpPost]

        public async Task<IActionResult> Submit(Setting model)
        {
            Setting? setting = _context.Settings.FirstOrDefault();
            if(setting == null)
            {
                await _context.Settings.AddAsync(model);
            }
            else
            {
                setting.Linkedin = model.Linkedin;
                setting.Email = model.Email;
                setting.Adress = model.Adress;
                setting.Phone = model.Phone;
                setting.Text = model.Text;
                setting.Twitter = model.Twitter;
                setting.Instagram = model.Instagram;
                setting.Facebook = model.Facebook;
            }
            await _context.SaveChangesAsync();
            TempData["Message"] = "Your setting has been updated successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
