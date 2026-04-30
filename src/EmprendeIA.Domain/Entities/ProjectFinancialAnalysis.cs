using System.ComponentModel.DataAnnotations;
using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Entities;

public class ProjectFinancialAnalysis
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    
    // AI generated sections (Markdown)
    public string RevenueProjections { get; private set; } = string.Empty;
    public string CostAnalysis { get; private set; } = string.Empty;
    public string BreakEvenAnalysis { get; private set; } = string.Empty;
    public string FundingRequirements { get; private set; } = string.Empty;
    public string KeyIndicators { get; private set; } = string.Empty;

    public DateTime GeneratedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public Project? Project { get; set; }

    private ProjectFinancialAnalysis() { }

    public ProjectFinancialAnalysis(Guid projectId)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        GeneratedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string revenueProjections,
        string costAnalysis,
        string breakEvenAnalysis,
        string fundingRequirements,
        string keyIndicators)
    {
        RevenueProjections = revenueProjections;
        CostAnalysis = costAnalysis;
        BreakEvenAnalysis = breakEvenAnalysis;
        FundingRequirements = fundingRequirements;
        KeyIndicators = keyIndicators;
        UpdatedAt = DateTime.UtcNow;
    }
}
