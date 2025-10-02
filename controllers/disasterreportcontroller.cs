using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class DisasterReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisasterReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DisasterReport
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view disaster reports.";
                return RedirectToAction("SignIn", "Account");
            }

            // Show all reports for admin users, or only user's reports for regular users
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");
            var reports = isAdmin 
                ? await _context.DisasterReports
                    .Include(r => r.ReportedByUser)
                    .OrderByDescending(r => r.DateReported)
                    .ToListAsync()
                : await _context.DisasterReports
                    .Where(r => r.ReportedByUserId == userId)
                    .Include(r => r.ReportedByUser)
                    .OrderByDescending(r => r.DateReported)
                    .ToListAsync();
                    
            return View(reports);
        }

        // GET: DisasterReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view disaster report details.";
                return RedirectToAction("SignIn", "Account");
            }

            var report = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
                return NotFound();

            // Check if user can view this report (owner or admin)
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");
            if (!isAdmin && report.ReportedByUserId != userId)
            {
                TempData["ErrorMessage"] = "You can only view your own disaster reports.";
                return RedirectToAction(nameof(Index));
            }

            return View(report);
        }

        // GET: DisasterReport/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to report a disaster.";
                return RedirectToAction("SignIn", "Account");
            }
            
            return View();
        }

        // POST: DisasterReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Location,DisasterType,Description")] DisasterReport report)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to report a disaster.";
                return RedirectToAction("SignIn", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    report.ReportedByUserId = userId;
                    report.DateReported = DateTime.Now;
                    
                    _context.Add(report);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Disaster report submitted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving disaster report: {ex.Message}");
                }
            }
            return View(report);
        }

        // GET: DisasterReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var report = await _context.DisasterReports.FindAsync(id);
            if (report == null)
                return NotFound();

            // Only allow report owner to edit
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != report.ReportedByUserId)
                return Forbid();

            return View(report);
        }

        // POST: DisasterReport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,DisasterType,DateReported,Description")] DisasterReport report)
        {
            if (id != report.Id)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to edit disaster reports.";
                return RedirectToAction("SignIn", "Account");
            }

            // Get the existing report to verify ownership
            var existingReport = await _context.DisasterReports.FindAsync(id);
            if (existingReport == null) return NotFound();
            
            // Check if the current user owns this report
            if (existingReport.ReportedByUserId != userId)
            {
                TempData["ErrorMessage"] = "You can only edit your own disaster reports.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure the user ID and date don't get changed during edit
                    report.ReportedByUserId = userId;
                    report.DateReported = existingReport.DateReported;
                    
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Disaster report updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterReportExists(report.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: DisasterReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to delete disaster reports.";
                return RedirectToAction("SignIn", "Account");
            }

            var report = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
                return NotFound();

            // Check if the current user owns this report
            if (report.ReportedByUserId != userId)
            {
                TempData["ErrorMessage"] = "You can only delete your own disaster reports.";
                return RedirectToAction(nameof(Index));
            }

            return View(report);
        }

        // POST: DisasterReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to delete disaster reports.";
                return RedirectToAction("SignIn", "Account");
            }

            var report = await _context.DisasterReports.FindAsync(id);
            if (report == null)
                return NotFound();

            // Check if the current user owns this report
            if (report.ReportedByUserId != userId)
            {
                TempData["ErrorMessage"] = "You can only delete your own disaster reports.";
                return RedirectToAction(nameof(Index));
            }

            _context.DisasterReports.Remove(report);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Disaster report deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterReportExists(int id)
        {
            return _context.DisasterReports.Any(e => e.Id == id);
        }
    }
}
