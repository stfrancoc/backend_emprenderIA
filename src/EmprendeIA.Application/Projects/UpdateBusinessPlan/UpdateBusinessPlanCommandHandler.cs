using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Application.Projects.UpdateBusinessPlan;

public class UpdateBusinessPlanCommandHandler : IRequestHandler<UpdateBusinessPlanCommand, bool>
{
    private readonly IBusinessPlanRepository _repository;
    private readonly IProjectRepository _projectRepository;

    public UpdateBusinessPlanCommandHandler(IBusinessPlanRepository repository, IProjectRepository projectRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(UpdateBusinessPlanCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId) return false;

        var plan = await _repository.GetByProjectIdAsync(request.ProjectId);
        if (plan == null)
        {
            await _repository.AddAsync(new BusinessPlan(request.ProjectId, request.Content));
        }
        else
        {
            plan.UpdateContent(request.Content);
            await _repository.UpdateAsync(plan);
        }
        return true;
    }
}
