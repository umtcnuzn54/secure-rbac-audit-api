using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecureApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string JwtKey =
        "SUPER_SECRET_DEMO_KEY_12345_abcdefghijklmnopqrstuvwxyz_123456";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (!InMemoryStore.LoginAttemptsByIp.ContainsKey(ip))
            InMemoryStore.LoginAttemptsByIp[ip] = new List<DateTime>();

        InMemoryStore.LoginAttemptsByIp[ip] = InMemoryStore.LoginAttemptsByIp[ip]
            .Where(t => (DateTime.UtcNow - t).TotalSeconds <= 60)
            .ToList();

        if (InMemoryStore.LoginAttemptsByIp[ip].Count >= 5)
        {
            InMemoryStore.AuditLogs.Add(new AuditLog
            {
                Username = req.Username,
                Role = "N/A",
                Method = "POST",
                Path = "/api/auth/login",
                StatusCode = 429,
                Ip = ip
            });

            return StatusCode(429, "Too many login attempts. Try again later.");
        }

        InMemoryStore.LoginAttemptsByIp[ip].Add(DateTime.UtcNow);

        var user = InMemoryStore.Users
            .FirstOrDefault(u => u.Username == req.Username && u.PasswordHash == req.Password);

        if (user == null)
        {
            InMemoryStore.AuditLogs.Add(new AuditLog
            {
                Username = req.Username,
                Role = "N/A",
                Method = "POST",
                Path = "/api/auth/login",
                StatusCode = 401,
                Ip = ip
            });

            return Unauthorized("Invalid credentials");
        }

        InMemoryStore.AuditLogs.Add(new AuditLog
        {
            Username = user.Username,
            Role = user.Role,
            Method = "POST",
            Path = "/api/auth/login",
            StatusCode = 200,
            Ip = ip
        });

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));

        var issuedAt = DateTime.UtcNow;
        var expiresAt = issuedAt.AddHours(2);

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: issuedAt,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            role = user.Role,
            issuedAt,
            expiresAt
        });
    }
}

public record LoginRequest(string Username, string Password);
