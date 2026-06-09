using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Entities;

public class Milestone
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public Project? Project { get; set; }

    private Milestone() { }

    public Milestone(Guid projectId, string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Title = title;
        Description = description;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ToggleCompletion()
    {
        IsCompleted = !IsCompleted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, DateTime dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }
}
