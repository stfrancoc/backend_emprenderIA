using MediatR;

namespace EmprendeIA.Application.Projects.GetByUser;

public record ProjectDto(
    Guid Id, 
    string Title, 
    string Description, 
    string Stage,
    string Status,
    DateTime CreatedAt,
    Guid OwnerId
);

public record GetUserProjectsQuery(Guid UserId) : IRequest<IEnumerable<ProjectDto>>;