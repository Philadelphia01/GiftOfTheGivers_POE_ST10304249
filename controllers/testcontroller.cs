using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Test database connection
                var donationCount = await _context.Donations.CountAsync();
                var volunteerTaskCount = await _context.VolunteerTasks.CountAsync();
                
                ViewBag.DonationCount = donationCount;
                ViewBag.VolunteerTaskCount = volunteerTaskCount;
                ViewBag.Status = "Database connection successful!";
                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Status = $"Database connection failed: {ex.Message}";
                return View();
            }
        }
    }
}
