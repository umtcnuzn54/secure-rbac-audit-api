using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApi.Models;

namespace SecureApi.Controllers;

[ApiController]
[Route("api/secure")]
public class SecureController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        AddAudit(200);
        return Ok("Admin access granted ✅");
    }

    [HttpGet("operator")]
    [Authorize(Roles = "Operator,Admin")]
    public IActionResult OperatorOrAdmin()
    {
        AddAudit(200);
        return Ok("Operator/Admin access granted ✅");
    }

    [HttpGet("viewer")]
    [Authorize(Roles = "Viewer,Operator,Admin")]
    public IActionResult AnyAuthenticated()
    {
        AddAudit(200);
        return Ok("Any authenticated role access granted ✅");
    }

    private void AddAudit(int statusCode)
    {
        var role = User.Claims.FirstOrDefault(c => c.Type.EndsWith("/role") || c.Type.Contains("role"))?.Value;

        InMemoryStore.AuditLogs.Add(new AuditLog
        {
            Username = User.Identity?.Name,
            Role = role,
            Method = Request.Method,
            Path = Request.Path,
            StatusCode = statusCode,
            Ip = HttpContext.Connection.RemoteIpAddress?.ToString()
        });
    }
}
