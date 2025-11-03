using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.UITests;

public class DonationControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly DonationController _controller;

    public DonationControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new DonationController(_context);
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
    public async Task Index_AuthenticatedRegularUser_ReturnsUserDonations()
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
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        var donation2 = new Donation { ResourceType = "Water", Quantity = 50, DateDonated = DateTime.Now, DonorUserId = "otherUser" };
        _context.Donations.AddRange(donation1, donation2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var donations = result.Model as List<Donation>;
        Assert.NotNull(donations);
        Assert.Single(donations);
        Assert.Equal("Food", donations.First().ResourceType);
    }

    [Fact]
    public async Task Index_AdminUser_ReturnsAllDonations()
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
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = "user1" };
        var donation2 = new Donation { ResourceType = "Water", Quantity = 50, DateDonated = DateTime.Now, DonorUserId = "user2" };
        _context.Donations.AddRange(donation1, donation2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var donations = result.Model as List<Donation>;
        Assert.NotNull(donations);
        Assert.Equal(2, donations.Count);
    }

    [Fact]
    public async Task Index_WithSearchFilter_ReturnsFilteredResults()
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
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        var donation2 = new Donation { ResourceType = "Water", Quantity = 50, DateDonated = DateTime.Now, DonorUserId = userId };
        _context.Donations.AddRange(donation1, donation2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index("Food") as ViewResult;

        // Assert
        Assert.NotNull(result);
        var donations = result.Model as List<Donation>;
        Assert.NotNull(donations);
        Assert.Single(donations);
        Assert.Equal("Food", donations.First().ResourceType);
        Assert.Equal("Food", _controller.ViewBag.SearchResourceType);
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Details(donation.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Donation;
        Assert.NotNull(model);
        Assert.Equal(donation.Id, model.Id);
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = "otherUser" };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Details(donation.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
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

        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 100,
            DateDonated = DateTime.Now,
            Note = "Test donation"
        };

        // Act
        var result = await _controller.Create(donation) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify donation was saved
        var savedDonation = await _context.Donations.FirstOrDefaultAsync(d => d.ResourceType == "Food");
        Assert.NotNull(savedDonation);
        Assert.Equal(userId, savedDonation.DonorUserId);
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

        var donation = new Donation
        {
            ResourceType = "", // Invalid
            Quantity = 100
        };
        _controller.ModelState.AddModelError("ResourceType", "Required");

        // Act
        var result = await _controller.Create(donation) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(donation, result.Model);
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Edit(donation.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Donation;
        Assert.NotNull(model);
        Assert.Equal(donation.Id, model.Id);
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        donation.ResourceType = "Updated Food";

        // Act
        var result = await _controller.Edit(donation.Id, donation) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify update
        var updatedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.Equal("Updated Food", updatedDonation.ResourceType);
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = userId };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteConfirmed(donation.Id) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify deletion
        var deletedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.Null(deletedDonation);
    }

    [Fact]
    public async Task Distribute_Get_ValidId_ReturnsView()
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = "user123", Status = "Pending" };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Distribute(donation.Id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as Donation;
        Assert.NotNull(model);
        Assert.Equal(donation.Id, model.Id);
    }

    [Fact]
    public async Task Distribute_Post_ValidData_UpdatesDonation()
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

        var donation = new Donation { ResourceType = "Food", Quantity = 100, DateDonated = DateTime.Now, DonorUserId = "user123", Status = "Pending" };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Distribute(donation.Id, "Distributed", "Cape Town", "Distributed to shelters") as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);

        // Verify update
        var updatedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.Equal("Distributed", updatedDonation.Status);
        Assert.Equal("Cape Town", updatedDonation.Location);
        Assert.Equal("Distributed to shelters", updatedDonation.DistributionNotes);
        Assert.NotNull(updatedDonation.DateDistributed);
        Assert.Equal(userId, updatedDonation.DistributedByUserId);
    }

    [Fact]
    public async Task Inventory_AdminUser_ReturnsView()
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
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, Status = "Pending" };
        var donation2 = new Donation { ResourceType = "Food", Quantity = 50, Status = "Approved" };
        var donation3 = new Donation { ResourceType = "Water", Quantity = 200, Status = "Distributed" };
        _context.Donations.AddRange(donation1, donation2, donation3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Inventory() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var inventory = result.Model as IEnumerable<dynamic>;
        Assert.NotNull(inventory);
    }

    [Fact]
    public async Task Inventory_NonAdminUser_RedirectsToIndex()
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
        var result = await _controller.Inventory() as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}
