using MediatR;

namespace EmprendeIA.Application.Projects.Create;

public record CreateProjectCommand(
    Guid OwnerId,
    string Title,
    string Description
) : IRequest<Guid>;