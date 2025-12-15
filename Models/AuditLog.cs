namespace SecureApi.Models;

public class AuditLog
{
    public long Id { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

    public string? Username { get; set; }
    public string? Role { get; set; }

    public string Method { get; set; } = "";
    public string Path { get; set; } = "";
    public int StatusCode { get; set; }

    public string? Ip { get; set; }
}
