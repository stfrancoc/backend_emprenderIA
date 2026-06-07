using System;

namespace EmprendeIA.Domain.Projects;

public class ProjectMatch
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid InvestorId { get; private set; }
    public decimal MatchScore { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public Project Project { get; private set; } = null!;

    private ProjectMatch() { } // Para EF Core

    public ProjectMatch(Guid projectId, Guid investorId, decimal matchScore)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        InvestorId = investorId;
        MatchScore = matchScore;
        CreatedAt = DateTime.UtcNow;
    }
}
