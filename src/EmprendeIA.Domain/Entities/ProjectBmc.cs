namespace EmprendeIA.Domain.Entities;

/// <summary>
/// Persists the generated Business Model Canvas for a project.
/// Maps to table project_bmc in modelo_relacional.
/// </summary>
public class ProjectBmc
{
    public Guid ProjectId { get; set; }
    public string CustomerSegments { get; set; } = string.Empty;
    public string ValueProposition { get; set; } = string.Empty;
    public string Channels { get; set; } = string.Empty;
    public string CustomerRelationships { get; set; } = string.Empty;
    public string RevenueStreams { get; set; } = string.Empty;
    public string KeyResources { get; set; } = string.Empty;
    public string KeyActivities { get; set; } = string.Empty;
    public string KeyPartners { get; set; } = string.Empty;
    public string CostStructure { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Projects.Project Project { get; set; } = null!;

    private ProjectBmc() { }

    public ProjectBmc(Guid projectId)
    {
        ProjectId = projectId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCanvas(
        string customerSegments, string valueProposition, string channels,
        string customerRelationships, string revenueStreams, string keyResources,
        string keyActivities, string keyPartners, string costStructure)
    {
        CustomerSegments = customerSegments;
        ValueProposition = valueProposition;
        Channels = channels;
        CustomerRelationships = customerRelationships;
        RevenueStreams = revenueStreams;
        KeyResources = keyResources;
        KeyActivities = keyActivities;
        KeyPartners = keyPartners;
        CostStructure = costStructure;
        UpdatedAt = DateTime.UtcNow;
    }
}
