using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class DisasterReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReportedByUserId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string DisasterType { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateReported { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        public ApplicationUser? ReportedByUser { get; set; }
    }
}
