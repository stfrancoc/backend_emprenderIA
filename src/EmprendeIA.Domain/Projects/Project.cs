namespace EmprendeIA.Domain.Projects;

using EmprendeIA.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProjectStage Stage { get; private set; } = ProjectStage.Idea;
    public ProjectStatus Status { get; private set; } = ProjectStatus.Activo;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation
    public ProjectBmc? Bmc { get; set; }

    private Project() { }

    public Project(Guid ownerId, string title, string description)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Description = description;
        Stage = ProjectStage.Idea;
        Status = ProjectStatus.Activo;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
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