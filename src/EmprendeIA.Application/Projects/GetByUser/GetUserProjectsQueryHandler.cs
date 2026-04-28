using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.GetByUser;

public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _repository;

    public GetUserProjectsQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetByOwnerIdAsync(request.UserId);

        return projects.Select(p => new ProjectDto(
            p.Id,
            p.Title,
            p.Description,
            p.Stage.ToString(),
            p.Status.ToString(),
            p.CreatedAt,
            p.OwnerId
        ));
    }
}