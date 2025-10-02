using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get dashboard statistics
            var totalUsers = await _userManager.Users.CountAsync();
            var totalDonations = await _context.Donations.CountAsync();
            var totalDisasterReports = await _context.DisasterReports.CountAsync();
            var totalVolunteerTasks = await _context.VolunteerTasks.CountAsync();
            var totalVolunteers = await _context.Volunteers.CountAsync();

            // Get recent activities
            var recentDonations = await _context.Donations
                .Include(d => d.DonorUser)
                .OrderByDescending(d => d.DateDonated)
                .Take(5)
                .ToListAsync();

            var recentReports = await _context.DisasterReports
                .Include(r => r.ReportedByUser)
                .OrderByDescending(r => r.DateReported)
                .Take(5)
                .ToListAsync();

            var recentTasks = await _context.VolunteerTasks
                .Include(t => t.AssignedVolunteer)
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalDonations = totalDonations;
            ViewBag.TotalDisasterReports = totalDisasterReports;
            ViewBag.TotalVolunteerTasks = totalVolunteerTasks;
            ViewBag.TotalVolunteers = totalVolunteers;
            ViewBag.RecentDonations = recentDonations;
            ViewBag.RecentReports = recentReports;
            ViewBag.RecentTasks = recentTasks;

            return View();
        }
    }
}
