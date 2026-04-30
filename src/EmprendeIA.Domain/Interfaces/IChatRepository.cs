using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IChatRepository
{
    Task<ChatSession?> GetSessionByIdAsync(Guid sessionId);
    Task<IEnumerable<ChatSession>> GetUserSessionsAsync(Guid userId);
    Task AddSessionAsync(ChatSession session);
    Task UpdateSessionAsync(ChatSession session);
    Task DeleteSessionAsync(Guid sessionId);
}
