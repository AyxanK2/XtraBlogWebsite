using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;
using XtraBlogWebsite.ViewsModel;

namespace XtraBlogWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.Link = "Home";
            List<Post> posts = _context.Posts
                        .Include(x => x.Category)
                        .Include(x=>x.Comment)
                        .Include(x => x.TagPosts)
                        .ThenInclude(x => x.Tag)
                        .Skip((page - 1)* 6)
                        .Take(6)
                        .ToList();
            if(posts is null) return NotFound();
            ViewBag.Page = page;
            ViewBag.Count = _context.Posts.ToList().Count;
            return View(posts);
        }
        public IActionResult About()
        {
            ViewBag.Link = "About";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Link = "Contact";
            Setting? setting = _context.Settings.FirstOrDefault();
            if (setting == null) return NotFound(); 
            if(setting == null)
            {
                return NotFound();
            }
            ContactVM model = new()
            {
                Setting = setting
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult SendMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["Message"] = "Your message sent successfully";
            return RedirectToAction(nameof(Contact));
        }
    }
}
