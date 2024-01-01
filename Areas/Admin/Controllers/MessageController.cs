using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Models;
using XtraBlogWebsite.Services;

namespace XtraBlogWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IEmailService _service;
        public MessageController(ApplicationContext context,IEmailService service)
        {
            _context = context;
            _service = service;
        }
        public IActionResult Index()
        {
            List<Message> messages = _context.Messages.ToList();
            return View(messages);
        }

        public async Task<IActionResult> Details(int id)
        {
            Message message = _context.Messages.FirstOrDefault(x=>x.Id == id);
            if (message == null)
            {
                return NotFound();
            }
            if (!message.Accepted)
            {
                await _service.SendMail(message.Email, "Bildiris", "Muracietiviz tesdiqlendi");
            }
            message.Accepted = true;
            _context.SaveChanges();
            return View(message);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            Message? message = _context.Messages.FirstOrDefault(x=>x.Id==id);
            if(message == null)
            {
                return NotFound();
            }
            await _service.SendMail(message.Email, "Bildiris", "Muracietiviz tesdiqlendi");
            message.Accepted = true;
            _context.SaveChanges();
            TempData["Message"] = "Message's status has been changed succsessfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Message? message = _context.Messages.FirstOrDefault(x=>x.Id==id);
            if (message == null)
            {
                return NotFound();
            }
            try
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
                return Json(new
                {
                    Message = "Message has been deleted successfully",
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
