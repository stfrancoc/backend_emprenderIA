namespace EmprendeIA.Domain.Entities;

public class ChatSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public string Title { get; private set; } = "Nueva Conversación";
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    private ChatSession() { }

    public ChatSession(Guid userId, Guid? projectId = null, string? title = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProjectId = projectId;
        Title = title ?? "Conversación " + DateTime.Now.ToString("g");
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddMessage(string role, string content)
    {
        Messages.Add(new ChatMessage(Id, role, content));
        UpdatedAt = DateTime.UtcNow;
    }
}

public class ChatMessage
{
    public Guid Id { get; private set; }
    public Guid ChatSessionId { get; private set; }
    public string Role { get; private set; } = string.Empty; // "user" or "assistant"
    public string Content { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }

    private ChatMessage() { }

    public ChatMessage(Guid chatSessionId, string role, string content)
    {
        Id = Guid.NewGuid();
        ChatSessionId = chatSessionId;
        Role = role;
        Content = content;
        Timestamp = DateTime.UtcNow;
    }
}
