using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;
using System;

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
        IEnumerable<EmprendeIA.Domain.Projects.Project> projects;

        if (!string.IsNullOrWhiteSpace(request.Role) &&
            request.Role.Equals("Entrepreneur", StringComparison.OrdinalIgnoreCase))
        {
            projects = await _repository.GetByOwnerIdAsync(request.UserId);
        }
        else if (!string.IsNullOrWhiteSpace(request.Role) &&
            (request.Role.Equals("Mentor", StringComparison.OrdinalIgnoreCase) ||
             request.Role.Equals("Investor", StringComparison.OrdinalIgnoreCase) ||
             request.Role.Equals("Inversor", StringComparison.OrdinalIgnoreCase)))
        {
            projects = await _repository.GetByMinimumStageAsync(ProjectStage.Prototipo);
        }
        else
        {
            // Default: return only owner's projects
            projects = await _repository.GetByOwnerIdAsync(request.UserId);
        }

        return projects.Select(p => new ProjectDto(
            p.Id,
            p.Title,
            p.Description,
            p.What,
            p.How,
            p.Why,
            p.ProjectType.ToString(),
            p.BusinessModelType.ToString(),
            p.Stage.ToString(),
            p.Status.ToString(),
            p.CreatedAt,
            p.OwnerId
        ));
    }
}