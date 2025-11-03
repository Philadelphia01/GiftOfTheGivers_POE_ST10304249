using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.UITests;

public class DisasterReportControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly DisasterReportController _controller;

    public DisasterReportControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new DisasterReportController(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Index_UnauthenticatedUser_RedirectsToSignIn()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.Index() as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SignIn", result.ActionName);
        Assert.Equal("Account", result.ControllerName);
    }

    [Fact]
    public async Task Index_AuthenticatedRegularUser_ReturnsUserReports()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Add test data
        var report1 = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        var report2 = new DisasterReport { Location = "Johannesburg", DisasterType = "Fire", Description = "Building fire", ReportedByUserId = "otherUser", DateReported = DateTime.Now };
        _context.DisasterReports.AddRange(report1, report2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var reports = result.Model as List<DisasterReport>;
        Assert.NotNull(reports);
        Assert.Single(reports);
        Assert.Equal("Cape Town", reports.First().Location);
    }

    [Fact]
    public async Task Index_AdminUser_ReturnsAllReports()
    {
        // Arrange
        var userId = "admin123";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Add test data
        var report1 = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = "user1", DateReported = DateTime.Now };
        var report2 = new DisasterReport { Location = "Johannesburg", DisasterType = "Fire", Description = "Building fire", ReportedByUserId = "user2", DateReported = DateTime.Now };
        _context.DisasterReports.AddRange(report1, report2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var reports = result.Model as List<DisasterReport>;
        Assert.NotNull(reports);
        Assert.Equal(2, reports.Count);
    }

    [Fact]
    public async Task Details_ValidId_ReturnsView()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Details(report.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as DisasterReport;
        Assert.NotNull(model);
        Assert.Equal(report.Id, model.Id);
    }

    [Fact]
    public async Task Details_InvalidId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_UnauthorizedUser_RedirectsToIndex()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = "otherUser", DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Details(report.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Create_Get_UnauthenticatedUser_RedirectsToSignIn()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.Create() as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SignIn", result.ActionName);
        Assert.Equal("Account", result.ControllerName);
    }

    [Fact]
    public void Create_Get_AuthenticatedUser_ReturnsView()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.Create() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Create_Post_ValidModel_RedirectsToIndex()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport
        {
            Location = "Cape Town",
            DisasterType = "Flood",
            Description = "Heavy flooding in residential areas"
        };

        // Act
        var result = await _controller.Create(report) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify report was saved
        var savedReport = await _context.DisasterReports.FirstOrDefaultAsync(r => r.Location == "Cape Town");
        Assert.NotNull(savedReport);
        Assert.Equal(userId, savedReport.ReportedByUserId);
        Assert.NotEqual(default(DateTime), savedReport.DateReported);
    }

    [Fact]
    public async Task Create_Post_InvalidModel_ReturnsView()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport
        {
            Location = "", // Invalid
            DisasterType = "Flood",
            Description = "Heavy flooding"
        };
        _controller.ModelState.AddModelError("Location", "Required");

        // Act
        var result = await _controller.Create(report) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(report, result.Model);
    }

    [Fact]
    public async Task Edit_Get_ValidId_ReturnsView()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Edit(report.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as DisasterReport;
        Assert.NotNull(model);
        Assert.Equal(report.Id, model.Id);
    }

    [Fact]
    public async Task Edit_Post_ValidModel_RedirectsToIndex()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        report.Location = "Updated Location";

        // Act
        var result = await _controller.Edit(report.Id, report) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify update
        var updatedReport = await _context.DisasterReports.FindAsync(report.Id);
        Assert.Equal("Updated Location", updatedReport.Location);
        Assert.Equal(report.DateReported, updatedReport.DateReported); // Date should not change
    }

    [Fact]
    public async Task Delete_Get_ValidId_ReturnsView()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Delete(report.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as DisasterReport;
        Assert.NotNull(model);
        Assert.Equal(report.Id, model.Id);
    }

    [Fact]
    public async Task Delete_Post_ValidId_RedirectsToIndex()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = userId, DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteConfirmed(report.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify deletion
        var deletedReport = await _context.DisasterReports.FindAsync(report.Id);
        Assert.Null(deletedReport);
    }

    [Fact]
    public async Task Delete_Post_UnauthorizedUser_RedirectsToIndex()
    {
        // Arrange
        var userId = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var report = new DisasterReport { Location = "Cape Town", DisasterType = "Flood", Description = "Heavy flooding", ReportedByUserId = "otherUser", DateReported = DateTime.Now };
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteConfirmed(report.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}
