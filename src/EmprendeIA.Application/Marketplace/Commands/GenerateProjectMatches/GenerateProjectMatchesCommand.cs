using MediatR;

namespace EmprendeIA.Application.Marketplace.Commands.GenerateProjectMatches;

public record GenerateProjectMatchesCommand(Guid ProjectId, Guid RequestingUserId, string BmcText) : IRequest<List<Guid>>;
