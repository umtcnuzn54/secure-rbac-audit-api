using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApi.Models;

namespace SecureApi.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Get([FromQuery] string? user, [FromQuery] string? path, [FromQuery] int limit = 50)
    {
        limit = Math.Clamp(limit, 1, 500);

        var query = InMemoryStore.AuditLogs.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(user))
            query = query.Where(l => string.Equals(l.Username, user, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(path))
            query = query.Where(l => (l.Path ?? "").Contains(path, StringComparison.OrdinalIgnoreCase));

        var logs = query
            .OrderByDescending(l => l.TimestampUtc)
            .Take(limit)
            .ToList();

        return Ok(new { count = logs.Count, logs });
    }
}
