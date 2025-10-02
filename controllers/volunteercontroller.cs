using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Volunteer
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                // User not logged in - redirect to login
                TempData["ErrorMessage"] = "You must be logged in to view volunteer tasks.";
                return RedirectToAction("SignIn", "Account");
            }
            
            // Only show volunteers for the current user
            var volunteers = await _context.Volunteers
                .Where(v => v.VolunteerUserId == userId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();
                
            return View(volunteers);
        }

        // GET: Volunteer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var volunteer = await _context.Volunteers.Include(v => v.VolunteerUser).FirstOrDefaultAsync(v => v.Id == id);
            if (volunteer == null)
                return NotFound();

            return View(volunteer);
        }

        // GET: Volunteer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Task,ScheduledDate,IsCompleted")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Challenge();

                volunteer.VolunteerUserId = userId;
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != volunteer.VolunteerUserId)
                return Forbid();

            return View(volunteer);
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Task,ScheduledDate,IsCompleted")] Volunteer volunteer)
        {
            if (id != volunteer.Id)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != volunteer.VolunteerUserId)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Volunteers.Any(v => v.Id == volunteer.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var volunteer = await _context.Volunteers.Include(v => v.VolunteerUser).FirstOrDefaultAsync(v => v.Id == id);
            if (volunteer == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != volunteer.VolunteerUserId)
                return Forbid();

            return View(volunteer);
        }

        // POST: Volunteer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != volunteer.VolunteerUserId)
                return Forbid();

            _context.Volunteers.Remove(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
