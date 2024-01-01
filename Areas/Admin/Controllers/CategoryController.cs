using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slugify;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationContext _context;

        public CategoryController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Edit(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(x=>x.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id,Category model)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();
            //category.Slug = model.Name.ToLower().Replace(' ', '-');
            SlugHelper helper = new SlugHelper();
            category.Slug = helper.GenerateSlug(category.Name);
            category.UpdateAt = DateTime.Now;
            category.Name = model.Name;
            _context.SaveChanges();
            TempData["Message"] = "Category has been updated succesfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            //category.Slug = category.Name.ToLower().Replace(' ', '-');
            SlugHelper helper = new SlugHelper();
            category.Slug = helper.GenerateSlug(category.Name);
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["Message"] = "Category has been created succesfully";
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
                if (category == null) return NotFound();
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Json(new
                {
                    Status = true,
                    Message = "Category has been deleted succesfully"
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
