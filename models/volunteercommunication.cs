using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerCommunication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderUserId { get; set; } = null!;

        public string? RecipientUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime? ReadAt { get; set; }

        public int? RelatedTaskId { get; set; }

        [StringLength(50)]
        public string MessageType { get; set; } = "General"; // General, Task Update, Emergency, Announcement

        public ApplicationUser? SenderUser { get; set; }
        public ApplicationUser? RecipientUser { get; set; }
        public VolunteerTask? RelatedTask { get; set; }
    }
}
