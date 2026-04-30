using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;
using System.Text.Json;

namespace EmprendeIA.Application.Assistant.Chat;

public class AssistantChatCommandHandler : IRequestHandler<AssistantChatCommand, ChatResponseDto>
{
    private readonly IAIService _aiService;
    private readonly IChatRepository _chatRepository;
    private readonly IProjectRepository _projectRepository;

    public AssistantChatCommandHandler(
        IAIService aiService, 
        IChatRepository chatRepository,
        IProjectRepository projectRepository)
    {
        _aiService = aiService;
        _chatRepository = chatRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ChatResponseDto> Handle(AssistantChatCommand request, CancellationToken cancellationToken)
    {
        ChatSession? session;

        if (request.SessionId.HasValue)
        {
            session = await _chatRepository.GetSessionByIdAsync(request.SessionId.Value);
            if (session == null || session.UserId != request.UserId)
                throw new Exception("Sesión no encontrada");
        }
        else
        {
            session = new ChatSession(request.UserId, request.ProjectId);
            await _chatRepository.AddSessionAsync(session);
        }

        // Contexto del proyecto si aplica
        string projectContext = "";
        if (session.ProjectId.HasValue)
        {
            var project = await _projectRepository.GetByIdAsync(session.ProjectId.Value);
            if (project != null)
            {
                projectContext = $"Proyecto: {project.Title}. Descripción: {project.Description}";
            }
        }

        // Historial de mensajes para la IA
        var history = session.Messages.OrderBy(m => m.Timestamp).TakeLast(10).Select(m => new
        {
            role = m.Role,
            content = m.Content
        }).ToList();

        var aiInput = new
        {
            message = request.Message,
            history = history,
            context = projectContext,
            user_id = request.UserId.ToString()
        };

        var aiResult = await _aiService.ChatAsync(aiInput);
        
        // Extraer respuesta
        var resultElement = aiResult is JsonElement element ? element : JsonSerializer.SerializeToElement(aiResult);
        string responseText = "";
        
        if (resultElement.TryGetProperty("response", out var respProp))
        {
            responseText = respProp.GetString() ?? "";
        }

        // Guardar mensajes
        session.AddMessage("user", request.Message);
        session.AddMessage("assistant", responseText);
        
        await _chatRepository.UpdateSessionAsync(session);

        return new ChatResponseDto(session.Id, responseText, session.Title);
    }
}
