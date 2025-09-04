using System.Security.Claims;
using BlazorRbacDemo.Models;

namespace BlazorRbacDemo.Services;

public interface IAuditLogger
{
    Task LogAsync(AuditAction action, int? orderId, string? details = null, ClaimsPrincipal? user = null);
}

public class AuditLogger : IAuditLogger
{
    private readonly AppDbContext _db;

    public AuditLogger(AppDbContext db) => _db = db;

    public async Task LogAsync(AuditAction action, int? orderId, string? details = null, ClaimsPrincipal? user = null)
    {
        var log = new AuditLog
        {
            Action = action,
            OrderId = orderId,
            Details = details,
            OccurredAtUtc = DateTime.UtcNow,
            PerformedByUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            PerformedByUserName = user?.Identity?.Name
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
