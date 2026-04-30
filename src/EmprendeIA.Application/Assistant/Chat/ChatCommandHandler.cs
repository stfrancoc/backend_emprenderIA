using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Assistant.Chat;

public class ChatCommandHandler : IRequestHandler<ChatCommand, object>
{
    private readonly IAIService _aiService;

    public ChatCommandHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<object> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        var aiInput = new 
        {
            project_id = request.ProjectId?.ToString() ?? "general",
            messages = request.Messages.Select(m => new { role = m.Role.ToLower(), content = m.Content }).ToList()
        };

        return await _aiService.ChatAsync(aiInput);
    }
}
