using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TagController : Controller
    {
        private readonly ApplicationContext _context;
        public TagController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Tag> tags = _context.Tags.ToList();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag model)
        {
            SlugHelper helper = new SlugHelper();
            model.Slug = helper.GenerateSlug(model.Name);
            _context.Tags.Add(model);
            _context.SaveChanges();
            TempData["Message"] = "Tag has been created succesfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Edit(int id,Tag model)
        {
            SlugHelper helper = new SlugHelper();
            Tag? tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null) return NotFound();
            tag.Slug = helper.GenerateSlug(model.Name);
            tag.UpdateAt = DateTime.Now;
            tag.Name = model.Name;
            _context.SaveChanges();
            TempData["Message"] = "Tag has been update succesfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Tag? tag = _context.Tags.FirstOrDefault(x=>x.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Tag? tag = _context.Tags.FirstOrDefault(x => x.Id == id);
                if (tag == null) return NotFound();
                _context.Tags.Remove(tag);
                _context.SaveChanges();
                return Json(new
                {
                    Status = true,
                    Message = "Tag has been deleted succesfully"
                });
            }
            catch
            {
                return Json(new
                {
                    Status = false,
                    Message = "Something went wrong"
                });
            }
        }
    }
}
