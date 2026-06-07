using MediatR;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.Create;

public class CreateProjectCommandHandler 
    : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _repository;

    public CreateProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = new Project(
            request.OwnerId,
            request.Title,
            request.Description,
            request.What,
            request.How,
            request.Why,
            request.ProjectType,
            request.BusinessModelType
        );

        await _repository.AddAsync(project);

        return project.Id;
    }
}