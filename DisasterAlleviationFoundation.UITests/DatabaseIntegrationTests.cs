using Xunit;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.UITests;

public class DatabaseIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public DatabaseIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateDonation_SavesToDatabase()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 100,
            DateDonated = DateTime.Now,
            DonorUserId = "user123",
            Note = "Test donation"
        };

        // Act
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Assert
        var savedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.NotNull(savedDonation);
        Assert.Equal("Food", savedDonation.ResourceType);
        Assert.Equal(100, savedDonation.Quantity);
        Assert.Equal("user123", savedDonation.DonorUserId);
    }

    [Fact]
    public async Task CreateVolunteer_SavesToDatabase()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            Task = "Food Distribution",
            ScheduledDate = DateTime.Now.AddDays(1),
            VolunteerUserId = "user123"
        };

        // Act
        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        // Assert
        var savedVolunteer = await _context.Volunteers.FindAsync(volunteer.Id);
        Assert.NotNull(savedVolunteer);
        Assert.Equal("Food Distribution", savedVolunteer.Task);
        Assert.Equal("user123", savedVolunteer.VolunteerUserId);
    }

    [Fact]
    public async Task CreateDisasterReport_SavesToDatabase()
    {
        // Arrange
        var report = new DisasterReport
        {
            Location = "Cape Town",
            DisasterType = "Flood",
            Description = "Heavy flooding in residential areas",
            ReportedByUserId = "user123",
            DateReported = DateTime.Now
        };

        // Act
        _context.DisasterReports.Add(report);
        await _context.SaveChangesAsync();

        // Assert
        var savedReport = await _context.DisasterReports.FindAsync(report.Id);
        Assert.NotNull(savedReport);
        Assert.Equal("Cape Town", savedReport.Location);
        Assert.Equal("Flood", savedReport.DisasterType);
        Assert.Equal("user123", savedReport.ReportedByUserId);
    }

    [Fact]
    public async Task CreateVolunteerTask_SavesToDatabase()
    {
        // Arrange
        var task = new VolunteerTask
        {
            Title = "Medical Aid Distribution",
            Description = "Distribute medical supplies to affected areas",
            StartAt = DateTime.Now.AddDays(1),
            EndAt = DateTime.Now.AddDays(2),
            AssignedVolunteerId = "user123",
            Status = "Open",
            Priority = "High"
        };

        // Act
        _context.VolunteerTasks.Add(task);
        await _context.SaveChangesAsync();

        // Assert
        var savedTask = await _context.VolunteerTasks.FindAsync(task.Id);
        Assert.NotNull(savedTask);
        Assert.Equal("Medical Aid Distribution", savedTask.Title);
        Assert.Equal("user123", savedTask.AssignedVolunteerId);
        Assert.Equal("Open", savedTask.Status);
    }

    [Fact]
    public async Task UpdateDonation_UpdatesInDatabase()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 100,
            DateDonated = DateTime.Now,
            DonorUserId = "user123"
        };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        donation.Quantity = 150;
        donation.Status = "Approved";
        await _context.SaveChangesAsync();

        // Assert
        var updatedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.Equal(150, updatedDonation.Quantity);
        Assert.Equal("Approved", updatedDonation.Status);
    }

    [Fact]
    public async Task DeleteDonation_RemovesFromDatabase()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 100,
            DateDonated = DateTime.Now,
            DonorUserId = "user123"
        };
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        // Act
        _context.Donations.Remove(donation);
        await _context.SaveChangesAsync();

        // Assert
        var deletedDonation = await _context.Donations.FindAsync(donation.Id);
        Assert.Null(deletedDonation);
    }

    [Fact]
    public async Task QueryDonationsByUserId_ReturnsCorrectResults()
    {
        // Arrange
        var donation1 = new Donation { ResourceType = "Food", Quantity = 100, DonorUserId = "user1" };
        var donation2 = new Donation { ResourceType = "Water", Quantity = 50, DonorUserId = "user1" };
        var donation3 = new Donation { ResourceType = "Medicine", Quantity = 25, DonorUserId = "user2" };
        _context.Donations.AddRange(donation1, donation2, donation3);
        await _context.SaveChangesAsync();

        // Act
        var user1Donations = await _context.Donations
            .Where(d => d.DonorUserId == "user1")
            .ToListAsync();

        // Assert
        Assert.Equal(2, user1Donations.Count);
        Assert.All(user1Donations, d => Assert.Equal("user1", d.DonorUserId));
    }

    [Fact]
    public async Task QueryVolunteerTasksByStatus_ReturnsCorrectResults()
    {
        // Arrange
        var task1 = new VolunteerTask { Title = "Task 1", Status = "Open", AssignedVolunteerId = "user1" };
        var task2 = new VolunteerTask { Title = "Task 2", Status = "In Progress", AssignedVolunteerId = "user1" };
        var task3 = new VolunteerTask { Title = "Task 3", Status = "Completed", AssignedVolunteerId = "user2" };
        _context.VolunteerTasks.AddRange(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var openTasks = await _context.VolunteerTasks
            .Where(t => t.Status == "Open")
            .ToListAsync();

        // Assert
        Assert.Single(openTasks);
        Assert.Equal("Task 1", openTasks.First().Title);
    }

    [Fact]
    public async Task ConcurrentDatabaseOperations_WorkCorrectly()
    {
        // Arrange
        var tasks = new List<Task>();

        // Act - Create multiple donations concurrently
        for (int i = 0; i < 10; i++)
        {
            var task = Task.Run(async () =>
            {
                var donation = new Donation
                {
                    ResourceType = $"Resource {Guid.NewGuid()}",
                    Quantity = 10,
                    DateDonated = DateTime.Now,
                    DonorUserId = "user123"
                };
                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        // Assert
        var totalDonations = await _context.Donations.CountAsync();
        Assert.Equal(10, totalDonations);
    }
}
