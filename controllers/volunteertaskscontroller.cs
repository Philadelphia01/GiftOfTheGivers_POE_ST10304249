using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class VolunteerTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                // User not logged in - redirect to login
                TempData["ErrorMessage"] = "You must be logged in to view volunteer tasks.";
                return RedirectToAction("SignIn", "Account");
            }
            
            Console.WriteLine($"Current user ID: {userId}");
            
            // Get all volunteer tasks for debugging
            var allTasks = await _context.VolunteerTasks.ToListAsync();
            Console.WriteLine($"Total tasks in database: {allTasks.Count}");
            foreach (var task in allTasks)
            {
                Console.WriteLine($"Task ID: {task.Id}, AssignedVolunteerId: {task.AssignedVolunteerId}, Title: {task.Title}");
            }
            
            // Clean up any orphaned tasks (tasks without proper user IDs)
            var orphanedTasks = await _context.VolunteerTasks
                .Where(vt => string.IsNullOrEmpty(vt.AssignedVolunteerId))
                .ToListAsync();
                
            if (orphanedTasks.Any())
            {
                Console.WriteLine($"Found {orphanedTasks.Count} orphaned tasks, removing them...");
                _context.VolunteerTasks.RemoveRange(orphanedTasks);
                await _context.SaveChangesAsync();
            }
            
            // Only show volunteer tasks for the current user
            var items = await _context.VolunteerTasks
                .Where(vt => vt.AssignedVolunteerId == userId)
                .OrderByDescending(t => t.StartAt)
                .ToListAsync();
                
            Console.WriteLine($"Tasks for current user: {items.Count}");
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view task details.";
                return RedirectToAction("SignIn", "Account");
            }

            var item = await _context.VolunteerTasks.FindAsync(id);
            if (item == null) return NotFound();
            
            // Check if the current user owns this task
            if (item.AssignedVolunteerId != userId)
            {
                TempData["ErrorMessage"] = "You can only view your own tasks.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(item);
        }

        public IActionResult Create() => View(new VolunteerTask());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerTask model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            
            try
            {
                // Get the current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (string.IsNullOrEmpty(userId))
                {
                    // User not logged in - redirect to login
                    TempData["ErrorMessage"] = "You must be logged in to create volunteer tasks.";
                    return RedirectToAction("SignIn", "Account");
                }
                
                // Assign the current user to the volunteer task
                model.AssignedVolunteerId = userId;
                
                Console.WriteLine($"Creating volunteer task for user: {userId}");
                Console.WriteLine($"Task title: {model.Title}");
                Console.WriteLine($"AssignedVolunteerId: {model.AssignedVolunteerId}");
                
                // Set default status if not provided
                if (string.IsNullOrEmpty(model.Status))
                    model.Status = "Open";
                    
                _context.VolunteerTasks.Add(model);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Volunteer task created successfully with ID: {model.Id}");
                TempData["SuccessMessage"] = "Volunteer task created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving volunteer task: {ex.Message}");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to edit tasks.";
                return RedirectToAction("SignIn", "Account");
            }

            var item = await _context.VolunteerTasks.FindAsync(id);
            if (item == null) return NotFound();
            
            // Check if the current user owns this task
            if (item.AssignedVolunteerId != userId)
            {
                TempData["ErrorMessage"] = "You can only edit your own tasks.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VolunteerTask model)
        {
            if (id != model.Id) return BadRequest();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to edit tasks.";
                return RedirectToAction("SignIn", "Account");
            }

            // Get the existing task to verify ownership
            var existingTask = await _context.VolunteerTasks.FindAsync(id);
            if (existingTask == null) return NotFound();
            
            // Check if the current user owns this task
            if (existingTask.AssignedVolunteerId != userId)
            {
                TempData["ErrorMessage"] = "You can only edit your own tasks.";
                return RedirectToAction(nameof(Index));
            }
            
            if (!ModelState.IsValid) return View(model);
            
            // Ensure the user ID doesn't get changed during edit
            model.AssignedVolunteerId = userId;
            
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Task updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to delete tasks.";
                return RedirectToAction("SignIn", "Account");
            }

            var item = await _context.VolunteerTasks.FindAsync(id);
            if (item == null) return NotFound();
            
            // Check if the current user owns this task
            if (item.AssignedVolunteerId != userId)
            {
                TempData["ErrorMessage"] = "You can only delete your own tasks.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to delete tasks.";
                return RedirectToAction("SignIn", "Account");
            }

            var item = await _context.VolunteerTasks.FindAsync(id);
            if (item != null)
            {
                // Check if the current user owns this task
                if (item.AssignedVolunteerId != userId)
                {
                    TempData["ErrorMessage"] = "You can only delete your own tasks.";
                    return RedirectToAction(nameof(Index));
                }
                
                _context.VolunteerTasks.Remove(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Task deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}


