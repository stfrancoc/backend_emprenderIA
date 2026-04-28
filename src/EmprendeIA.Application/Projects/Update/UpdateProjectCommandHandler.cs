using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.Update;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IProjectRepository _repository;
    public UpdateProjectCommandHandler(IProjectRepository repository) => _repository = repository;

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);

        if (project == null || project.OwnerId != request.OwnerId)
            return false;

        project.Update(request.Title, request.Description);

        await _repository.UpdateAsync(project);
        return true;
    }
}