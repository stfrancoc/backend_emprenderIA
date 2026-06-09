using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using System.Security.Claims;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/forum")]
[Authorize]
public class ForumController : ControllerBase
{
    private readonly IForumRepository _repository;

    public ForumController(IForumRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("topics")]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await _repository.GetAllTopicsAsync();
        return Ok(topics);
    }

    [HttpPost("topics")]
    public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        if (userId == Guid.Empty)
            return Unauthorized("User ID not found in token");

        var topic = new ForumTopic(userId, request.Title, request.Content);
        await _repository.AddTopicAsync(topic);
        return CreatedAtAction(nameof(GetTopics), topic);
    }

    [HttpGet("topics/{topicId}/replies")]
    public async Task<IActionResult> GetReplies(Guid topicId)
    {
        var topic = await _repository.GetTopicByIdAsync(topicId);
        if (topic == null)
            return NotFound("Topic not found");

        var replies = await _repository.GetRepliesByTopicIdAsync(topicId);
        return Ok(replies);
    }

    [HttpPost("topics/{topicId}/replies")]
    public async Task<IActionResult> CreateReply(Guid topicId, [FromBody] CreateReplyRequest request)
    {
        var topic = await _repository.GetTopicByIdAsync(topicId);
        if (topic == null)
            return NotFound("Topic not found");

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        if (userId == Guid.Empty)
            return Unauthorized("User ID not found in token");

        var reply = new ForumReply(topicId, userId, request.Content);
        await _repository.AddReplyAsync(reply);
        return CreatedAtAction(nameof(GetReplies), new { topicId }, reply);
    }
}

public class CreateTopicRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class CreateReplyRequest
{
    public string Content { get; set; } = string.Empty;
}
