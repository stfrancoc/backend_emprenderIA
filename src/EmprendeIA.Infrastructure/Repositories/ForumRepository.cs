using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmprendeIA.Infrastructure.Repositories;

public class ForumRepository : IForumRepository
{
    private readonly ApplicationDbContext _context;

    public ForumRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ForumTopic?> GetTopicByIdAsync(Guid topicId)
        => await _context.ForumTopics.Include(t => t.Replies).FirstOrDefaultAsync(t => t.Id == topicId);

    public async Task<IEnumerable<ForumTopic>> GetAllTopicsAsync()
        => await _context.ForumTopics.Include(t => t.User).OrderByDescending(t => t.CreatedAt).ToListAsync();

    public async Task AddTopicAsync(ForumTopic topic)
    {
        await _context.ForumTopics.AddAsync(topic);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTopicAsync(ForumTopic topic)
    {
        _context.ForumTopics.Update(topic);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ForumReply>> GetRepliesByTopicIdAsync(Guid topicId)
        => await _context.ForumReplies.Where(r => r.TopicId == topicId).Include(r => r.User).OrderBy(r => r.CreatedAt).ToListAsync();

    public async Task AddReplyAsync(ForumReply reply)
    {
        await _context.ForumReplies.AddAsync(reply);
        await _context.SaveChangesAsync();

        // Increment reply count on topic
        var topic = await GetTopicByIdAsync(reply.TopicId);
        if (topic != null)
        {
            topic.IncrementReplyCount();
            await UpdateTopicAsync(topic);
        }
    }

    public async Task DeleteTopicAsync(Guid topicId)
    {
        var topic = await GetTopicByIdAsync(topicId);
        if (topic != null)
        {
            _context.ForumTopics.Remove(topic);
            await _context.SaveChangesAsync();
        }
    }
}
