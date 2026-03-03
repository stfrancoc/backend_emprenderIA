namespace EmprendeIA.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private User() { }  

    public User(string email, string passwordHash, string role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}