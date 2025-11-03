using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.UITests;

public class AdminDashboardControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly AdminDashboardController _controller;

    public AdminDashboardControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        // Mock UserManager
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _controller = new AdminDashboardController(_context, _userManagerMock.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Index_UnauthenticatedUser_RedirectsToLogin()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsType<ChallengeResult>(result);
    }

    [Fact]
    public async Task Index_NonAdminUser_RedirectsToLogin()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "user123") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsType<ChallengeResult>(result);
    }

    [Fact]
    public async Task Index_AdminUser_ReturnsViewWithStatistics()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "admin123"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Mock UserManager to return user count
        _userManagerMock.Setup(um => um.Users.CountAsync(default))
            .ReturnsAsync(5);

        // Add test data
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = "user1" };
        var donation2 = new Donation { ResourceType = "Water", Quantity = 50, DateDonated = DateTime.Now, DonorUserId = "user2" };
        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = "user1", DateReported = DateTime.Now };
        var task = new VolunteerTask { Title = "Medical Aid", AssignedVolunteerId = "user1", CreatedAt = DateTime.Now };
        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = "user1" };

        _context.Donations.AddRange(donation1, donation2);
        _context.DisasterReports.Add(report);
        _context.VolunteerTasks.Add(task);
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);

        // Check ViewBag values
        Assert.Equal(5, _controller.ViewBag.TotalUsers);
        Assert.Equal(2, _controller.ViewBag.TotalDonations);
        Assert.Equal(1, _controller.ViewBag.TotalDisasterReports);
        Assert.Equal(1, _controller.ViewBag.TotalVolunteerTasks);
        Assert.Equal(1, _controller.ViewBag.TotalVolunteers);

        // Check recent data
        var recentDonations = _controller.ViewBag.RecentDonations as List<Donation>;
        Assert.NotNull(recentDonations);
        Assert.Equal(2, recentDonations.Count);

        var recentReports = _controller.ViewBag.RecentReports as List<DisasterReport>;
        Assert.NotNull(recentReports);
        Assert.Single(recentReports);

        var recentTasks = _controller.ViewBag.RecentTasks as List<VolunteerTask>;
        Assert.NotNull(recentTasks);
        Assert.Single(recentTasks);
    }
}
