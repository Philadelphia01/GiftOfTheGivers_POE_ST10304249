using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DisasterAlleviationFoundation.Controllers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.UITests;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_loggerMock.Object);
    }

    [Fact]
    public void Index_UnauthenticatedUser_RedirectsToRegister()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity()); // No claims = unauthenticated
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.Index() as RedirectToPageResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("/Account/Register", result.PageName);
    }

    [Fact]
    public void Index_AuthenticatedNonAdminUser_ReturnsView()
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
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Index_AdminUser_RedirectsToAdminDashboard()
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

        // Act
        var result = _controller.Index() as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("AdminDashboard", result.ControllerName);
    }

    [Fact]
    public void Index_AdminUserWithViewWebsite_ReturnsView()
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

        // Act
        var result = _controller.Index(viewWebsite: true) as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void About_ReturnsView()
    {
        // Act
        var result = _controller.About() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Contact_ReturnsView()
    {
        // Act
        var result = _controller.Contact() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Privacy_ReturnsView()
    {
        // Act
        var result = _controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Error_ReturnsView()
    {
        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void TestDb_ReturnsJsonWithSuccess()
    {
        // Arrange
        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = _controller.TestDb() as JsonResult;

        // Assert
        Assert.NotNull(result);
        dynamic data = result.Value;
        Assert.True((bool)data.success);
    }
}
