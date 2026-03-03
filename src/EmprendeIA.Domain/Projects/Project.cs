namespace EmprendeIA.Domain.Projects;

public class Project
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }

    public DateTime CreatedAt { get; private set; }
    private Project() { }

    public Project(Guid ownerId, string title, string description)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}