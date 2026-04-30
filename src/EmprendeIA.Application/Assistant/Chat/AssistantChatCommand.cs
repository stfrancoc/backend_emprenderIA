using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Assistant.Chat;

public record AssistantChatCommand(string Message, Guid? SessionId = null, Guid? ProjectId = null) : IRequest<ChatResponseDto>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}

public record ChatResponseDto(Guid SessionId, string Response, string Title);
