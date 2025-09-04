using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorRbacDemo.Models;

public enum OrderStatus { Draft, Submitted, Approved, Rejected }

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 1_000_000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    // Ownership / auditing
    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAtUtc { get; set; }

    // Optimistic concurrency
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}
