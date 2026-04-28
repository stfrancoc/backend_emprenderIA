namespace EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Profiles;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    // 2FA / TOTP
    public bool Is2FAEnabled { get; private set; } = false;
    public string? TotpSecret { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation properties
    public UserProfile? UserProfile { get; set; }
    public EntrepreneurProfile? EntrepreneurProfile { get; set; }
    public InvestorProfile? InvestorProfile { get; set; }
    public MentorProfile? MentorProfile { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    private User() { }

    public User(string name, string email, string passwordHash, string role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Enable2FA(string totpSecret)
    {
        Is2FAEnabled = true;
        TotpSecret = totpSecret;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Disable2FA()
    {
        Is2FAEnabled = false;
        TotpSecret = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}