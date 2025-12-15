using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApi.Models;

namespace SecureApi.Controllers;

[ApiController]
[Route("api/command")]
public class CommandController : ControllerBase
{
    [HttpPost("lock-target")]
    [Authorize(Roles = "Operator,Admin")]
    public IActionResult LockTarget([FromBody] TargetRequest req)
    {
        InMemoryStore.AuditLogs.Add(new AuditLog
        {
            Username = User.Identity?.Name,
            Role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value,
            Method = "POST",
            Path = "/api/command/lock-target",
            StatusCode = 200,
            Ip = HttpContext.Connection.RemoteIpAddress?.ToString()
        });

        return Ok(new
        {
            message = $"Target {req.TargetId} locked successfully.",
            timestamp = DateTime.UtcNow
        });
    }
}

public record TargetRequest(string TargetId);
