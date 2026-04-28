namespace EmprendeIA.Domain.Entities;

public class UserProfile
{
    public Guid UserId { get; set; }
    public string? Bio { get; set; }
    public List<string> Skills { get; set; } = new();
    public List<string> Interests { get; set; } = new();
    public string ExperienceLevel { get; set; } = "junior"; // junior, intermedio, senior
    public List<string> Industries { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;

    private UserProfile() { }

    public UserProfile(Guid userId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? bio, List<string>? skills, List<string>? interests,
        string? experienceLevel, List<string>? industries)
    {
        if (bio is not null) Bio = bio;
        if (skills is not null) Skills = skills;
        if (interests is not null) Interests = interests;
        if (experienceLevel is not null) ExperienceLevel = experienceLevel;
        if (industries is not null) Industries = industries;
        UpdatedAt = DateTime.UtcNow;
    }
}
