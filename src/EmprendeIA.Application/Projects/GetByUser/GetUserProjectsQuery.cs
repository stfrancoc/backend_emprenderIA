using MediatR;

namespace EmprendeIA.Application.Projects.GetByUser;

public record ProjectDto(
    Guid Id, 
    string Title, 
    string Description, 
    DateTime CreatedAt,
    Guid OwnerId
);

public record GetUserProjectsQuery(Guid UserId) : IRequest<IEnumerable<ProjectDto>>;