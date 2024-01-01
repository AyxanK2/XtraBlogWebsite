using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;
using XtraBlogWebsite.Services;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationContext _context;
        public CommentController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Comment> comments = _context.Comments.ToList();
            return View(comments);
        }

        public async Task<IActionResult> Details(int id)
        {
            Comment? comment = await _context.Comments
                                        .Include(x=>x.Post)
                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _context.SaveChanges();
            return View(comment);
        }

        public async Task<IActionResult> ChangeStatus(int id,int status)
        {
            Comment? comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            if (status == 0) comment.Accepted = false;
            else comment.Accepted = true;
            _context.SaveChanges();
            TempData["Message"] = "Comment's verified successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Comment? comment = _context.Comments.FirstOrDefault(x => x.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            try
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Comment has been deleted successfully",
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
