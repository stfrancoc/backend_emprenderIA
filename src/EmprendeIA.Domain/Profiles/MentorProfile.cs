namespace EmprendeIA.Domain.Profiles;
using EmprendeIA.Domain.Entities;

public class MentorProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Specialties { get; set; } = null!;
    public int YearsExperience { get; set; }

    public User User { get; set; } = null!;
}