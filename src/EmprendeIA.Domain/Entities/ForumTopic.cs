namespace EmprendeIA.Domain.Entities;

public class ForumTopic
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int ReplyCount { get; private set; } = 0;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public User? User { get; set; }
    public ICollection<ForumReply> Replies { get; set; } = new List<ForumReply>();

    private ForumTopic() { }

    public ForumTopic(Guid userId, string title, string content)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementReplyCount()
    {
        ReplyCount++;
        UpdatedAt = DateTime.UtcNow;
    }
}
