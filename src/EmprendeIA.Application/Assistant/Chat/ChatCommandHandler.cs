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
            ProjectId = request.ProjectId,
            Messages = request.Messages
        };

        return await _aiService.ChatAsync(aiInput);
    }
}
