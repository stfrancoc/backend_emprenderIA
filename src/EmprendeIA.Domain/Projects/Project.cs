namespace EmprendeIA.Domain.Projects;

using EmprendeIA.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string What { get; private set; } = string.Empty;
    public string How { get; private set; } = string.Empty;
    public string Why { get; private set; } = string.Empty;
    public ProjectType ProjectType { get; private set; } = ProjectType.Producto;
    public BusinessModelType BusinessModelType { get; private set; } = BusinessModelType.Necesidad;
    public ProjectStage Stage { get; private set; } = ProjectStage.Idea;
    public ProjectStatus Status { get; private set; } = ProjectStatus.Activo;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation
    public ProjectBmc? Bmc { get; set; }
    public ProjectFinancialAnalysis? FinancialAnalysis { get; set; }
    public BusinessPlan? BusinessPlan { get; set; }

    private Project() { }

    public Project(Guid ownerId, string title, string description, string what, string how, string why, ProjectType projectType, BusinessModelType businessModelType)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Description = description;
        What = what;
        How = how;
        Why = why;
        ProjectType = projectType;
        BusinessModelType = businessModelType;
        Stage = ProjectStage.Idea;
        Status = ProjectStatus.Activo;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, string what, string how, string why, ProjectType projectType, BusinessModelType businessModelType)
    {
        Title = title;
        Description = description;
        What = what;
        How = how;
        Why = why;
        ProjectType = projectType;
        BusinessModelType = businessModelType;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStage(ProjectStage stage)
    {
        Stage = stage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = ProjectStatus.Archivado;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}