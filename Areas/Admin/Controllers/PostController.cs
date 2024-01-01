using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Slugify;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Extensions;
using XtraBlogWebsite.Models;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;
        public PostController(ApplicationContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Post> posts = _context.Posts
                                .Include(x=>x.Category)
                                .Include(x=>x.TagPosts)
                                .ThenInclude(x=>x.Tag)
                                .ToList();
            return View(posts);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();  
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Post post)
        {
            SlugHelper helper = new SlugHelper();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if(post.File == null)
            {
                ModelState.AddModelError("File", "Please select a photo");
                return View(post);
            }
            if (post.File.Length / 1024 > 500) ModelState.AddModelError("File", "File's length must be less than 500kb");
            if (!post.File.ContentType.Contains("image")) ModelState.AddModelError("File", "File format must be an image");
            if(!ModelState.IsValid) return View(post);

            post.Image = await post.File.FileUpload(_env.WebRootPath, "posts");
            post.Slug = helper.GenerateSlug(post.Title);
            post.TagPosts = new List<TagPost>();
            foreach (int tagid in post.TagIds)
            {
                post.TagPosts.Add(new TagPost { TagId = tagid });
            }
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Post has been added succesfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();   
            ViewBag.Tags = _context.Tags.ToList();
            Post? posts = _context.Posts
                             .Include(x=>x.TagPosts)
                             .ThenInclude(x=>x.Tag)
                             .FirstOrDefault(x=>x.Id == id);
            if (posts == null) return NotFound();
            return View(posts);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id,Post model)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            SlugHelper helper = new SlugHelper();
            if (!ModelState.IsValid) return NotFound();
            Post? post = await _context.Posts
                .Include(x=>x.TagPosts)
                .ThenInclude(x=>x.Tag)
                .Include(x=>x.Category)
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (post is null) return NotFound();
            if(model.File != null)
            {
                if (model.File.Length / 1024 > 200) ModelState.AddModelError("File", "File's length must be less than 200kb");
                if (!model.File.ContentType.Contains("image")) ModelState.AddModelError("File", "File format must be an image");

                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads", "posts", post.Image)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads", "posts", post.Image));
                }
                post.Image = await model.File.FileUpload(_env.WebRootPath, "posts");
            }

            List<int> postTagIds = post.TagPosts.Select(x=>x.TagId).ToList();
            List<int> addTagIds = model.TagIds.FindAll(x => !postTagIds.Contains(x));
            List<int> removeTagIds = postTagIds.FindAll(x => !model.TagIds.Contains(x));
            post.TagPosts.RemoveAll(x => removeTagIds.Contains(x.TagId));
            foreach (int tagid in addTagIds)
            {
                post.TagPosts.Add(new TagPost { TagId = tagid });
            }

            post.Title = model.Title;
            post.Slug = helper.GenerateSlug(model.Title);
            post.CategoryId = model.CategoryId;
            post.Date = model.Date;
            post.LongDesc = model.LongDesc;
            post.ShortDesc = model.ShortDesc;
            post.UpdateAt = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Post has been updated succesfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Post? post = _context.Posts.FirstOrDefault(x => x.Id == id);
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads", "posts",post.Image)))
                {
                    System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads", "posts",post.Image));
                }
                _context.Posts.Remove(post);
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Post has been deleted successfully ",
                    Status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    Message = "Something went wrong",
                    Status = false
                });
            }
        }
    }
}
