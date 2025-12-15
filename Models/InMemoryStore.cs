namespace SecureApi.Models;

public static class InMemoryStore
{
    public static List<AppUser> Users = new()
    {
        new AppUser
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "admin123", 
            Role = "Admin"
        },
        new AppUser
        {
            Id = 2,
            Username = "operator",
            PasswordHash = "operator123",
            Role = "Operator"
        }
    };

    public static List<AuditLog> AuditLogs = new();
    public static Dictionary<string, List<DateTime>> LoginAttemptsByIp = new();

}
