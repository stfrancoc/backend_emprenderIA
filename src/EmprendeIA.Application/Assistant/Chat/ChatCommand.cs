using MediatR;

namespace EmprendeIA.Application.Assistant.Chat;

public record ChatCommand(Guid? ProjectId, List<ChatMessageDto> Messages) : IRequest<object>;

public record ChatMessageDto(string Role, string Content);
