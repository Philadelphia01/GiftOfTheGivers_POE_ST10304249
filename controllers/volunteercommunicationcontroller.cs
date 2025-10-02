using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class VolunteerCommunicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerCommunicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerCommunication - View all communications
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view communications.";
                return RedirectToAction("SignIn", "Account");
            }

            var communications = await _context.VolunteerCommunications
                .Where(c => c.RecipientUserId == userId || c.SenderUserId == userId)
                .Include(c => c.SenderUser)
                .Include(c => c.RecipientUser)
                .Include(c => c.RelatedTask)
                .OrderByDescending(c => c.SentAt)
                .ToListAsync();

            return View(communications);
        }

        // GET: VolunteerCommunication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var communication = await _context.VolunteerCommunications
                .Include(c => c.SenderUser)
                .Include(c => c.RecipientUser)
                .Include(c => c.RelatedTask)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (communication == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (communication.RecipientUserId != userId && communication.SenderUserId != userId)
            {
                return Forbid();
            }

            // Mark as read if current user is the recipient
            if (communication.RecipientUserId == userId && !communication.IsRead)
            {
                communication.IsRead = true;
                communication.ReadAt = DateTime.Now;
                _context.Update(communication);
                await _context.SaveChangesAsync();
            }

            return View(communication);
        }

        // GET: VolunteerCommunication/Create
        public async Task<IActionResult> Create(int? taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to send a message.";
                return RedirectToAction("SignIn", "Account");
            }

            var communication = new VolunteerCommunication
            {
                SenderUserId = userId,
                RelatedTaskId = taskId
            };

            // Get list of users for recipient selection
            var users = await _context.Users
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            ViewBag.Users = users;
            ViewBag.MessageTypes = new[] { "General", "Task Update", "Emergency", "Announcement" };

            return View(communication);
        }

        // POST: VolunteerCommunication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerCommunication communication)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to send a message.";
                return RedirectToAction("SignIn", "Account");
            }

            if (ModelState.IsValid)
            {
                communication.SenderUserId = userId;
                communication.SentAt = DateTime.Now;
                communication.IsRead = false;

                _context.Add(communication);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Message sent successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Reload users for the view
            var users = await _context.Users
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            ViewBag.Users = users;
            ViewBag.MessageTypes = new[] { "General", "Task Update", "Emergency", "Announcement" };

            return View(communication);
        }

        // GET: VolunteerCommunication/Reply/5
        public async Task<IActionResult> Reply(int? id)
        {
            if (id == null)
                return NotFound();

            var originalMessage = await _context.VolunteerCommunications
                .Include(c => c.SenderUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (originalMessage == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (originalMessage.SenderUserId != userId && originalMessage.RecipientUserId != userId)
            {
                return Forbid();
            }

            var reply = new VolunteerCommunication
            {
                SenderUserId = userId,
                RecipientUserId = originalMessage.SenderUserId,
                Subject = "Re: " + originalMessage.Subject,
                RelatedTaskId = originalMessage.RelatedTaskId,
                MessageType = originalMessage.MessageType
            };

            return View(reply);
        }

        // POST: VolunteerCommunication/Reply/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, VolunteerCommunication communication)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to send a message.";
                return RedirectToAction("SignIn", "Account");
            }

            if (ModelState.IsValid)
            {
                communication.SenderUserId = userId;
                communication.SentAt = DateTime.Now;
                communication.IsRead = false;

                _context.Add(communication);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reply sent successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(communication);
        }

        // GET: VolunteerCommunication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var communication = await _context.VolunteerCommunications
                .Include(c => c.SenderUser)
                .Include(c => c.RecipientUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (communication == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (communication.SenderUserId != userId)
            {
                return Forbid();
            }

            return View(communication);
        }

        // POST: VolunteerCommunication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var communication = await _context.VolunteerCommunications.FindAsync(id);
            if (communication == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (communication.SenderUserId != userId)
            {
                return Forbid();
            }

            _context.VolunteerCommunications.Remove(communication);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Message deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
