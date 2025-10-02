using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Start Date & Time")]
        public DateTime? StartAt { get; set; }

        [Display(Name = "End Date & Time")]
        public DateTime? EndAt { get; set; }

        [StringLength(40)]
        public string Status { get; set; } = "Open"; // Open, Assigned, In Progress, Completed, Cancelled

        [Display(Name = "Assigned Volunteer Id")]
        public string? AssignedVolunteerId { get; set; }

        [StringLength(500)]
        [Display(Name = "Notes / Updates")]
        public string? Notes { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [StringLength(50)]
        public string? Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

        [StringLength(100)]
        public string? Category { get; set; } // Emergency Response, Food Distribution, Medical Aid, etc.

        [Range(1, 100)]
        public int? MaxVolunteers { get; set; } = 1;

        [Range(0, 100)]
        public int CurrentVolunteerCount { get; set; } = 0;

        [StringLength(200)]
        public string? RequiredSkills { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedByUserId { get; set; }

        public ApplicationUser? AssignedVolunteer { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
    }
}


