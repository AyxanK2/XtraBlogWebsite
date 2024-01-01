using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;
using XtraBlogWebsite.ViewsModel;

namespace XtraBlogWebsite.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationContext _context;
        public PostController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Details(string slug)
        {
            Post? post = _context.Posts.Include(x => x.TagPosts)
                                        .ThenInclude(x => x.Tag)
                                        .Include(x=>x.Comment)
                                        .FirstOrDefault(x => x.Slug == slug);
            if (post is null) return NotFound();

            PostVM model = new PostVM()
            {
                Post = post,
                Categories = _context.Categories.ToList(),
                RelatiedPosts = _context.Posts
                                .Where(x => x.CategoryId == post.CategoryId)
                                .Where(x=>x.Id != post.Id)
                                .OrderByDescending(x=>x.Id)
                                .Take(3)
                                .ToList()
            };
            return View(model);
        }

        [HttpPost]

        public IActionResult SendComment(string slug,Comment comment)
        {
            Post? post = _context.Posts.Include(x => x.TagPosts)
                                        .ThenInclude(x => x.Tag)
                                        .FirstOrDefault(x => x.Slug == slug);
            if (post is null) return NotFound();
            post.Comment = new List<Comment>();
            _context.Comments.Add(new Comment
            {
                Email = comment.Email,
                Name = comment.Name,
                Text = comment.Text,
                PostId = post.Id,
            });
            _context.SaveChanges();
            TempData["Message"] = "Please,wait to confirm your comment";
            return RedirectToAction(nameof(Details), new { slug = slug });
        }
    }
}
