using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.Delete;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _repository;
    public DeleteProjectCommandHandler(IProjectRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);

        if (project == null || project.OwnerId != request.OwnerId)
            return false;

        await _repository.DeleteAsync(project);
        return true;
    }
}