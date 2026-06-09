namespace EmprendeIA.Domain.Entities;

public class ForumReply
{
    public Guid Id { get; private set; }
    public Guid TopicId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public ForumTopic? Topic { get; set; }
    public User? User { get; set; }

    private ForumReply() { }

    public ForumReply(Guid topicId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        TopicId = topicId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string content)
    {
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
}
