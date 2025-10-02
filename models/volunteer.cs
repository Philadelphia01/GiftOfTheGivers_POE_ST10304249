using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Volunteer
    {
        [Key]
        public int Id { get; set; }

        public string? VolunteerUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Task { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime ScheduledDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        public ApplicationUser? VolunteerUser { get; set; }
    }
}
