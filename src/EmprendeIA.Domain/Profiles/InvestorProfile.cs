namespace EmprendeIA.Domain.Profiles;
using EmprendeIA.Domain.Entities;

public class InvestorProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal InvestmentRangeMin { get; set; }
    public decimal InvestmentRangeMax { get; set; }

    public string Interests { get; set; } = null!;

    public User User { get; set; } = null!;
}