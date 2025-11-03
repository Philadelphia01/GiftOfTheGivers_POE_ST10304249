using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using DisasterAlleviationFoundation;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DisasterAlleviationFoundation.UITests;

public class ApiEndpointIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiEndpointIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Home_Index_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Disaster Alleviation Foundation", content);
    }

    [Fact]
    public async Task Home_About_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Home/About");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Home_Contact_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Home/Contact");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Home_Privacy_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Home/Privacy");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Donation_Index_Unauthenticated_RedirectsToLogin()
    {
        // Act
        var response = await _client.GetAsync("/Donation");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task Volunteer_Index_Unauthenticated_RedirectsToLogin()
    {
        // Act
        var response = await _client.GetAsync("/Volunteer");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task DisasterReport_Index_Unauthenticated_RedirectsToLogin()
    {
        // Act
        var response = await _client.GetAsync("/DisasterReport");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task VolunteerTasks_Index_Unauthenticated_RedirectsToLogin()
    {
        // Act
        var response = await _client.GetAsync("/VolunteerTasks");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task AdminDashboard_Index_Unauthenticated_RedirectsToLogin()
    {
        // Act
        var response = await _client.GetAsync("/AdminDashboard");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task Home_TestDb_ReturnsJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/Home/TestDb");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<TestDbResponse>();
        Assert.NotNull(result);
        Assert.True(result.success);
    }

    [Fact]
    public async Task StaticFiles_Css_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/css/site.css");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/css", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task StaticFiles_Js_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/js/site.js");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/javascript", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task InvalidUrl_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/InvalidUrl");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task HttpPost_ToGetOnlyEndpoint_ReturnsMethodNotAllowed()
    {
        // Arrange
        var content = new StringContent("");

        // Act
        var response = await _client.PostAsync("/Home/About", content);

        // Assert
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task LargeRequest_HandledProperly()
    {
        // Arrange
        var largeContent = new string('a', 10000);
        var content = new StringContent(largeContent);

        // Act
        var response = await _client.PostAsync("/Home/About", content);

        // Assert
        // Should either succeed or return method not allowed, but not crash
        Assert.True(response.StatusCode == HttpStatusCode.MethodNotAllowed ||
                   response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async Task MultipleConcurrentRequests_HandledProperly()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/"));
        }

        // Act
        var responses = await Task.WhenAll(tasks);

        // Assert
        Assert.All(responses, r => Assert.Equal(HttpStatusCode.OK, r.StatusCode));
    }

    private class TestDbResponse
    {
        public bool success { get; set; }
        public bool canConnect { get; set; }
        public string connectionString { get; set; } = string.Empty;
    }
}
