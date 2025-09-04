using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorRbacDemo.Models;

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

    // what
    [Required]
    public AuditAction Action { get; set; }

    // when
    [Required]
    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;

    // who
    [MaxLength(256)]
    public string? PerformedByUserId { get; set; }

    [MaxLength(256)]
    public string? PerformedByUserName { get; set; } // convenience for display

    // which order (nullable if you ever log non-order actions)
    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    // freeform details (what changed, etc.)
    [MaxLength(2000)]
    public string? Details { get; set; }
}
