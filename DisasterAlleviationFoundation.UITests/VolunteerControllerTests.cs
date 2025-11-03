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

public class VolunteerControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly VolunteerController _controller;

    public VolunteerControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new VolunteerController(_context);
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
    public async Task Index_AuthenticatedUser_ReturnsUserVolunteers()
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
        var volunteer1 = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        var volunteer2 = new Volunteer { Task = "Medical Aid", ScheduledDate = DateTime.Now, VolunteerUserId = "otherUser" };
        _context.Volunteers.AddRange(volunteer1, volunteer2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var volunteers = result.Model as List<Volunteer>;
        Assert.NotNull(volunteers);
        Assert.Single(volunteers);
        Assert.Equal("Food Distribution", volunteers.First().Task);
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Details(volunteer.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Volunteer;
        Assert.NotNull(model);
        Assert.Equal(volunteer.Id, model.Id);
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
    public void Create_Get_ReturnsView()
    {
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

        var volunteer = new Volunteer
        {
            Task = "Food Distribution",
            ScheduledDate = DateTime.Now.AddDays(1)
        };

        // Act
        var result = await _controller.Create(volunteer) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify volunteer was saved
        var savedVolunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Task == "Food Distribution");
        Assert.NotNull(savedVolunteer);
        Assert.Equal(userId, savedVolunteer.VolunteerUserId);
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

        var volunteer = new Volunteer
        {
            Task = "", // Invalid
            ScheduledDate = DateTime.Now
        };
        _controller.ModelState.AddModelError("Task", "Required");

        // Act
        var result = await _controller.Create(volunteer) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(volunteer, result.Model);
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Edit(volunteer.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Volunteer;
        Assert.NotNull(model);
        Assert.Equal(volunteer.Id, model.Id);
    }

    [Fact]
    public async Task Edit_Get_UnauthorizedUser_ReturnsForbid()
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = "otherUser" };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Edit(volunteer.Id);

        // Assert
        Assert.IsType<ForbidResult>(result);
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        volunteer.Task = "Updated Task";

        // Act
        var result = await _controller.Edit(volunteer.Id, volunteer) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify update
        var updatedVolunteer = await _context.Volunteers.FindAsync(volunteer.Id);
        Assert.Equal("Updated Task", updatedVolunteer.Task);
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Delete(volunteer.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Volunteer;
        Assert.NotNull(model);
        Assert.Equal(volunteer.Id, model.Id);
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

        var volunteer = new Volunteer { Task = "Food Distribution", ScheduledDate = DateTime.Now, VolunteerUserId = userId };
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteConfirmed(volunteer.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify deletion
        var deletedVolunteer = await _context.Volunteers.FindAsync(volunteer.Id);
        Assert.Null(deletedVolunteer);
    }
}
