namespace EmprendeIA.Domain.Profiles;
using EmprendeIA.Domain.Entities;

public class EntrepreneurProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Portfolio { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int ExperienceYears { get; set; }

    public User User { get; set; } = null!;
}