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
            var reports = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .ToListAsync();
            return View(reports);
        }

        // GET: DisasterReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var report = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
                return NotFound();

            return View(report);
        }

        // GET: DisasterReport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DisasterReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Location,DisasterType,Description")] DisasterReport report)
        {
            Console.WriteLine("Create action called");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"UserId: {userId}");
            if (userId == null)
                return Challenge();

            report.ReportedByUserId = userId;
            report.DateReported = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(report);

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(report);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception saving report: {ex.Message}");
                    return View(report);
                }
                return RedirectToAction(nameof(Index));
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
            if (userId != report.ReportedByUserId)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
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

            var report = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != report.ReportedByUserId)
                return Forbid();

            return View(report);
        }

        // POST: DisasterReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.DisasterReports.FindAsync(id);
            if (report == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != report.ReportedByUserId)
                return Forbid();

            _context.DisasterReports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterReportExists(int id)
        {
            return _context.DisasterReports.Any(e => e.Id == id);
        }
    }
}
