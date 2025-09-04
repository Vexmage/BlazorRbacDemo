using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorRbacDemo.Models
{
    public enum AuditAction
    {
        Created = 1,
        Updated = 2,
        Submitted = 3,
        Approved = 4,
        Rejected = 5,
        Deleted = 6,
        Exported = 7
    }

    public class AuditLog
    {
        public int Id { get; set; }

        // What happened
        [Required]
        public AuditAction Action { get; set; }

        // When it happened (UTC)
        [Required]
        public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;

        // Who did it (ids are strings in ASP.NET Identity)
        [MaxLength(256)]
        public string? PerformedByUserId { get; set; }

        [MaxLength(256)]
        public string? PerformedByUserName { get; set; }

        // Which order (nullable to allow non-order events like CSV export)
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        // Optional details (diff/notes)
        [MaxLength(2000)]
        public string? Details { get; set; }
    }
}
