using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Domain.Interfaces;

public interface IForumRepository
{
    Task<ForumTopic?> GetTopicByIdAsync(Guid topicId);
    Task<IEnumerable<ForumTopic>> GetAllTopicsAsync();
    Task AddTopicAsync(ForumTopic topic);
    Task UpdateTopicAsync(ForumTopic topic);
    Task<IEnumerable<ForumReply>> GetRepliesByTopicIdAsync(Guid topicId);
    Task AddReplyAsync(ForumReply reply);
    Task DeleteTopicAsync(Guid topicId);
}
