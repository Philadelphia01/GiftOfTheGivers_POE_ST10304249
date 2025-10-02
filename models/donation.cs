using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        public string? DonorUserId { get; set; }

        [Required]
        [StringLength(200)]
        public string ResourceType { get; set; } = null!;

        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateDonated { get; set; }

        [StringLength(1000)]
        public string? Note { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Distributed, Cancelled

        [StringLength(200)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? DistributionNotes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateDistributed { get; set; }

        public string? DistributedByUserId { get; set; }

        public ApplicationUser? DonorUser { get; set; }
        public ApplicationUser? DistributedByUser { get; set; }
    }
}
