using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class TaskBrowseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskBrowseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskBrowse - Browse available tasks for volunteers
        public async Task<IActionResult> Index(string? category, string? priority, string? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to browse tasks.";
                return RedirectToAction("SignIn", "Account");
            }

            var query = _context.VolunteerTasks
                .Where(t => t.Status == "Open" || t.Status == "Assigned")
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(t => t.Category == category);
            }

            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priority == priority);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            var tasks = await query
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.StartAt)
                .ToListAsync();

            // Add filter options for the view
            ViewBag.Categories = await _context.VolunteerTasks
                .Where(t => !string.IsNullOrEmpty(t.Category))
                .Select(t => t.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Priorities = new[] { "Low", "Medium", "High", "Critical" };
            ViewBag.Statuses = new[] { "Open", "Assigned" };

            return View(tasks);
        }

        // GET: TaskBrowse/Details/5 - View task details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var task = await _context.VolunteerTasks
                .Include(t => t.AssignedVolunteer)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: TaskBrowse/Join/5 - Join a task as a volunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to join a task.";
                return RedirectToAction("SignIn", "Account");
            }

            var task = await _context.VolunteerTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            // Check if task is still open and has capacity
            if (task.Status != "Open" && task.Status != "Assigned")
            {
                TempData["ErrorMessage"] = "This task is no longer available.";
                return RedirectToAction(nameof(Index));
            }

            if (task.CurrentVolunteerCount >= (task.MaxVolunteers ?? 1))
            {
                TempData["ErrorMessage"] = "This task is full.";
                return RedirectToAction(nameof(Index));
            }

            // Assign the volunteer to the task
            task.AssignedVolunteerId = userId;
            task.Status = "Assigned";
            task.CurrentVolunteerCount = task.CurrentVolunteerCount + 1;

            _context.Update(task);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully joined the task!";
            return RedirectToAction(nameof(Index));
        }

        // POST: TaskBrowse/Leave/5 - Leave a task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to leave a task.";
                return RedirectToAction("SignIn", "Account");
            }

            var task = await _context.VolunteerTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            if (task.AssignedVolunteerId != userId)
            {
                TempData["ErrorMessage"] = "You are not assigned to this task.";
                return RedirectToAction(nameof(Index));
            }

            // Remove the volunteer from the task
            task.AssignedVolunteerId = null;
            task.Status = "Open";
            task.CurrentVolunteerCount = Math.Max(0, task.CurrentVolunteerCount - 1);

            _context.Update(task);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully left the task.";
            return RedirectToAction(nameof(Index));
        }
    }
}
