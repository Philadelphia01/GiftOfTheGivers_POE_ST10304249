using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundation.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Donation
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                // User not logged in - redirect to login
                TempData["ErrorMessage"] = "You must be logged in to view donations.";
                return RedirectToAction("SignIn", "Account");
            }
            
            // Show all donations for admin users, or only user's donations for regular users
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");
            var donations = isAdmin 
                ? await _context.Donations
                    .Include(d => d.DonorUser)
                    .Include(d => d.DistributedByUser)
                    .OrderByDescending(d => d.DateDonated)
                    .ToListAsync()
                : await _context.Donations
                    .Where(d => d.DonorUserId == userId)
                    .Include(d => d.DonorUser)
                    .Include(d => d.DistributedByUser)
                    .OrderByDescending(d => d.DateDonated)
                    .ToListAsync();
                
            return View(donations);
        }

        // GET: Donation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations.Include(d => d.DonorUser).FirstOrDefaultAsync(d => d.Id == id);
            if (donation == null)
                return NotFound();

            return View(donation);
        }

        // GET: Donation/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Donation/Test
        public IActionResult Test()
        {
            return View();
        }

        // GET: Donation/TestDirect
        public async Task<IActionResult> TestDirect()
        {
            try
            {
                var testDonation = new Donation
                {
                    ResourceType = "Test Food",
                    Quantity = 10,
                    DateDonated = DateTime.Now,
                    Note = "Direct test donation",
                    DonorUserId = "test-user"
                };

                _context.Add(testDonation);
                await _context.SaveChangesAsync();
                
                ViewBag.Message = "Direct test donation created successfully!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View();
            }
        }

        // POST: Donation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation donation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user ID
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    
                    if (string.IsNullOrEmpty(userId))
                    {
                        // User not logged in - redirect to login
                        TempData["ErrorMessage"] = "You must be logged in to make a donation.";
                        return RedirectToAction("SignIn", "Account");
                    }
                    
                    // Set the user ID for the donation
                    donation.DonorUserId = userId;
                    
                    // Set default date if not provided
                    if (donation.DateDonated == default)
                        donation.DateDonated = DateTime.Now;
                    
                    _context.Add(donation);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Donation created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving donation: {ex.Message}");
                }
            }
            return View(donation);
        }

        // GET: Donation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != donation.DonorUserId)
                return Forbid();

            return View(donation);
        }

        // POST: Donation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceType,Quantity,DateDonated,Note")] Donation donation)
        {
            if (id != donation.Id)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != donation.DonorUserId)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Donations.Any(d => d.Id == donation.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        // GET: Donation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations.Include(d => d.DonorUser).FirstOrDefaultAsync(d => d.Id == id);
            if (donation == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != donation.DonorUserId)
                return Forbid();

            return View(donation);
        }

        // POST: Donation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != donation.DonorUserId)
                return Forbid();

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Donation/Distribute/5
        public async Task<IActionResult> Distribute(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            // Check if user is admin or the donor
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");
            
            if (!isAdmin && userId != donation.DonorUserId)
                return Forbid();

            return View(donation);
        }

        // POST: Donation/Distribute/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Distribute(int id, [Bind("Id,Status,Location,DistributionNotes")] Donation donation)
        {
            if (id != donation.Id)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");

            if (!isAdmin)
            {
                TempData["ErrorMessage"] = "Only administrators can distribute donations.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDonation = await _context.Donations.FindAsync(id);
                    if (existingDonation == null)
                        return NotFound();

                    existingDonation.Status = donation.Status;
                    existingDonation.Location = donation.Location;
                    existingDonation.DistributionNotes = donation.DistributionNotes;
                    
                    if (donation.Status == "Distributed")
                    {
                        existingDonation.DateDistributed = DateTime.Now;
                        existingDonation.DistributedByUserId = userId;
                    }

                    _context.Update(existingDonation);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Donation status updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Donations.Any(d => d.Id == donation.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(donation);
        }

        // GET: Donation/Inventory - View donation inventory
        public async Task<IActionResult> Inventory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view inventory.";
                return RedirectToAction("SignIn", "Account");
            }

            var isAdmin = User.IsInRole("Admin") || User.IsInRole("Administrator");
            if (!isAdmin)
            {
                TempData["ErrorMessage"] = "Only administrators can view inventory.";
                return RedirectToAction(nameof(Index));
            }

            var inventory = await _context.Donations
                .Where(d => d.Status != "Cancelled")
                .GroupBy(d => d.ResourceType)
                .Select(g => new
                {
                    ResourceType = g.Key,
                    TotalQuantity = g.Sum(d => d.Quantity),
                    PendingQuantity = g.Where(d => d.Status == "Pending").Sum(d => d.Quantity),
                    ApprovedQuantity = g.Where(d => d.Status == "Approved").Sum(d => d.Quantity),
                    DistributedQuantity = g.Where(d => d.Status == "Distributed").Sum(d => d.Quantity)
                })
                .OrderBy(i => i.ResourceType)
                .ToListAsync();

            return View(inventory);
        }
    }
}
