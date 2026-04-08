using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Application.Projects.GetByUser;

namespace EmprendeIA.Application.Projects.GetById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectRepository _repository;

    public GetProjectByIdQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);

        if (project == null) return null;

        return new ProjectDto(
            project.Id,
            project.Title,
            project.Description,
            project.CreatedAt,
            project.OwnerId
        );
    }
}