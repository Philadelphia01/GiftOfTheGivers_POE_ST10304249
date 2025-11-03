using Xunit;
using DisasterAlleviationFoundation.Models;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.UITests;

public class ModelTests
{
    [Fact]
    public void Donation_ValidModel_ShouldPassValidation()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 100,
            DateDonated = DateTime.Now,
            DonorUserId = "user123"
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Donation_InvalidResourceType_ShouldFailValidation()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "", // Invalid: empty string
            Quantity = 100,
            DateDonated = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("ResourceType"));
    }

    [Fact]
    public void Donation_InvalidQuantity_ShouldFailValidation()
    {
        // Arrange
        var donation = new Donation
        {
            ResourceType = "Food",
            Quantity = 0, // Invalid: below minimum
            DateDonated = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Quantity"));
    }

    [Fact]
    public void Volunteer_ValidModel_ShouldPassValidation()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            Task = "Food Distribution",
            ScheduledDate = DateTime.Now.AddDays(1),
            VolunteerUserId = "user123"
        };

        // Act
        var validationResults = ValidateModel(volunteer);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Volunteer_InvalidTask_ShouldFailValidation()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            Task = "", // Invalid: empty
            ScheduledDate = DateTime.Now.AddDays(1)
        };

        // Act
        var validationResults = ValidateModel(volunteer);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Task"));
    }

    [Fact]
    public void DisasterReport_ValidModel_ShouldPassValidation()
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
        var validationResults = ValidateModel(report);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void DisasterReport_InvalidLocation_ShouldFailValidation()
    {
        // Arrange
        var report = new DisasterReport
        {
            Location = "", // Invalid: empty
            DisasterType = "Flood",
            Description = "Heavy flooding",
            DateReported = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(report);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Location"));
    }

    [Fact]
    public void VolunteerTask_ValidModel_ShouldPassValidation()
    {
        // Arrange
        var task = new VolunteerTask
        {
            Title = "Medical Aid Distribution",
            Description = "Distribute medical supplies to affected areas",
            StartAt = DateTime.Now.AddDays(1),
            EndAt = DateTime.Now.AddDays(2),
            AssignedVolunteerId = "user123"
        };

        // Act
        var validationResults = ValidateModel(task);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void VolunteerTask_InvalidTitle_ShouldFailValidation()
    {
        // Arrange
        var task = new VolunteerTask
        {
            Title = "", // Invalid: empty
            Description = "Description",
            StartAt = DateTime.Now.AddDays(1)
        };

        // Act
        var validationResults = ValidateModel(task);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Title"));
    }

    [Fact]
    public void ApplicationUser_InheritsFromIdentityUser()
    {
        // Arrange
        var user = new ApplicationUser
        {
            UserName = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            Address = "123 Test St"
        };

        // Assert
        Assert.NotNull(user.UserName);
        Assert.NotNull(user.Email);
        Assert.Equal("Test User", user.FullName);
        Assert.Equal("123 Test St", user.Address);
    }

    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}
